using GameService.Inter;
using GameService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using GameService.OnlineUserManager;
using Microsoft.AspNetCore.Authorization;
using GameService.GameRepositories;
using System.Collections.Generic;
using GameLib;
using GameLib.Models;

namespace GameService.Hubs
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {
        private readonly IOnlineUserManager onlineUserManager;
        private readonly IGameRepository gameRepository;
        public GameHub(IOnlineUserManager onlineUserManager, IGameRepository gameRepository)
        {
            this.onlineUserManager = onlineUserManager;
            this.gameRepository = gameRepository;
        }

        public override async Task OnConnectedAsync()
        {
            //check if new user
            var user = await onlineUserManager.GetUserAsync(Context.UserIdentifier);
            if (user != null)
            {
                //user connected twice (new connection id)
                if (!user.ConnectionIds.Contains(Context.ConnectionId))
                    user.ConnectionIds.Add(Context.ConnectionId);
                return;
            }

            //add the new user
            user = new User
            {
                UserID = Context.UserIdentifier,
                ConnectionIds = new List<string> { Context.ConnectionId }
            };
            await onlineUserManager.AddLiveUser(user);

            //alert the lobby of a new user
            await Clients.AllExcept(Context.ConnectionId).UserJoinedLobbyAsync(user);

            //send back the full lobby
            await Clients.Caller.ReciveFullLobbyAsync(await onlineUserManager.GetAvailabeUsers());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //check if user is not connected
            bool isUserConnected = await onlineUserManager.IsUserConnected(Context.UserIdentifier);
            if (!isUserConnected)
            {
                return;
            }

            //remove the user
            var user = await onlineUserManager.RemoveLiveUser(Context.ConnectionId);

            //alert the lobby of a dissconnect
            await Clients.AllExcept(Context.ConnectionId).UserLeftLobbyAsync(user);

        }


        public async Task RequestGame(string reciverUsername)
        {
            //check user online and available
            User sender = await onlineUserManager.GetUserAsync(Context.ConnectionId);
            User reciver = await onlineUserManager.GetUserAsync(reciverUsername);

            //check users online
            if (sender is null || reciver is null) return;

            //send the request with the sender's identifier
            await Clients.User(reciverUsername).GameRequested(Context.UserIdentifier);
        }
        public async Task AcceptGame(string senderUsername)
        {
            User sender = await onlineUserManager.GetUserAsync(senderUsername);
            User reciver = await onlineUserManager.GetUserAsync(Context.UserIdentifier);

            //check users online
            if (sender is null || reciver is null) return;

            //create the game 
            var gameView = await gameRepository.GenerateGameAsync(senderUsername);

            //update the game for the players
            await onlineUserManager.UpdateUserGame(sender.UserID, senderUsername);
            await onlineUserManager.UpdateUserGame(reciver.UserID, senderUsername);

            //add to group
            //add all connections for both the sender and the reciver
            var connectionIds = sender
                                    .ConnectionIds
                                    .Concat(reciver.ConnectionIds)
                                    .ToList();

            connectionIds.ForEach(async cnn =>
                await Groups.AddToGroupAsync(cnn, senderUsername));

            //send them the game
           await Clients.Group(senderUsername).ReceiveGameView(gameView);
        }

        public async Task StartGame()
        {
            //load the users and verify
            var user1 = await onlineUserManager.GetUserAsync(Context.UserIdentifier);
            var user2 = await onlineUserManager.GetOpponentAsync(user1);
            if (user1 is null || user2 is null) return;

            //load the game
            BackgammonLogic game = await gameRepository.GetGame(user1.GameID);
            if (game is null) return;

            //orgenize data for players
            var player1 = new Player
            {
                Name = user1.UserID,
                Direction = DirectionEnum.ClockWise
            };
            var player2 = new Player
            {
                Name = user2.UserID,
                Direction = DirectionEnum.AntiClockWise
            };
            //preform startGame action
            var turn = game.StartGame(player1, player2);

            //return the turn 
            await Clients.Caller.ReceiveTurn(turn);
        }
        public async Task PreformAction(GameAction action)
        {
            var user = await onlineUserManager.GetUserAsync(Context.UserIdentifier);
            var game = await gameRepository.GetGame(user.GameID);
            var turn  = game.PerformAction(action);
        }
        //public async Task Invite(User host, IEnumerable<string> users)
        //{
        //    if (host.GameID == Guid.Empty)
        //    {
        //        await Clients.Caller.ErrorHandling("No host found");
        //        return;
        //    }

        //    var game = await _gameRepository.GetGameAsync(host.GameID);

        //    if (game is null)
        //    {
        //        await Clients.Caller.ErrorHandling("No game found");
        //        return;
        //    }

        //    await Clients.Users(users).InvitedToGameAsync(game.GameID.ToString());
        //}

        //public async Task CreateGameAsync(User user)
        //{
        //    var game = await _gameRepository.GenerateGameAsync();

        //    user.GameID = game.GameID;

        //    await Groups.AddToGroupAsync(Context.ConnectionId, user.GameID.ToString());
        //    await UdateUserManager(game);

        //    await Clients.Group(user.GameID.ToString()).GameCreatedAsync(game);
        //}

        //public async Task JoinGameAsync(User user)
        //{
        //    var game = await _gameRepository.GetGameAsync(user.GameID);

        //    if (game is null) return;
        //    await UdateUserManager(game);
        //    await Groups.AddToGroupAsync(Context.ConnectionId, game.GameID.ToString());
        //}


        //public async Task LeaveGameAsync(User user)
        //{
        //    var game = await _gameRepository.GetGameAsync(user.GameID);
        //    if (game is null) return;
        //    await UdateUserManager(null);
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());
        //}

        //public async Task CloseGameAsync(User user)
        //{
        //    var game = await _gameRepository.GetGameAsync(user.GameID);

        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());

        //    await _gameRepository.CloseGameAsync(game.GameID);
        //}
        //private async Task UdateUserManager(GameLib.Models.Game game) =>
        //    await _onlineUserManager.UpdateUserGame(Context.ConnectionId, game?.GameID ?? Guid.Empty);
    }
}
