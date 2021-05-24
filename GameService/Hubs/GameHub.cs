using GameService.Inter;
using GameService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using GameService.OnlineUserManager;
using Microsoft.AspNetCore.Authorization;
using GameService.GameRepositories;

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
            bool isUserConnected = await onlineUserManager.IsUserConnected(Context.UserIdentifier);
            if (isUserConnected)
            {
                return;
            }

            //add the new user
            var user = new User { UserID = Context.UserIdentifier };
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


        public async Task RequestGame(User receiver)
        {
            //check user online and available
            var isConnected = await onlineUserManager.IsUserConnected(receiver.UserID);
            if (!isConnected)
            {
                return;
            }
            bool isAvailable = await onlineUserManager.IsUserAvailable(receiver.UserID);
            if (!isAvailable)
            {
                return;
            }
            //send the request with the sender's identifier
            await Clients.User(receiver.UserID).GameRequested(Context.UserIdentifier);
        }
        public async Task AcceptGame(string senderUsername)
        {
            //check user online and available
            var isConnected = await onlineUserManager.IsUserConnected(senderUsername);
            if (!isConnected)
            {
                return;
            }
            bool isAvailable = await onlineUserManager.IsUserAvailable(senderUsername);
            if (!isAvailable)
            {
                return;
            }
            //create the game and add them both to it
            //send them the game
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
