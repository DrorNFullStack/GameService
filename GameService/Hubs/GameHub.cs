using GameService.Inter;
using GameService.GameRepositories;
using GameService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.OnlineUserManager;

namespace GameService.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IOnlineUserManager _onlineUserManager;
        public GameHub(IGameRepository gameRepository, IOnlineUserManager onlineUserManager)
        {
            _gameRepository = gameRepository;
            this._onlineUserManager = onlineUserManager;
        }

        public override async Task OnConnectedAsync()
        {
            await _onlineUserManager.AddLiveUser(new User { UserID = Context.ConnectionId });
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _onlineUserManager.RemoveLiveUser(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RequestGame(User receiver, User sender) 
        {
            await Clients.User(receiver.UserID).GameRequested(sender);
        }

        public async Task Invite(User host, IEnumerable<string> users)
        {
            if (host.GameID == Guid.Empty)
            {
                await Clients.Caller.ErrorHandling("No host found");
                return;
            }

            var game = await _gameRepository.GetGameAsync(host.GameID);

            if (game is null)
            {
                await Clients.Caller.ErrorHandling("No game found");
                return;
            }

            await Clients.Users(users).InvitedToGameAsync(game.GameID.ToString());
        }

        public async Task CreateGameAsync(User user)
        {
            var game = await _gameRepository.GenerateGameAsync();

            user.GameID = game.GameID;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, user.GameID.ToString());
            await UdateUserManager(game);

            await Clients.Group(user.GameID.ToString()).GameCreatedAsync(game);
        }

        public async Task JoinGameAsync(User user)
        {
            var game = await _gameRepository.GetGameAsync(user.GameID);

            if (game is null) return;
            await UdateUserManager(game);
            await Groups.AddToGroupAsync(Context.ConnectionId, game.GameID.ToString());
        }


        public async Task LeaveGameAsync(User user)
        {
            var game = await _gameRepository.GetGameAsync(user.GameID);
            if (game is null) return;
            await UdateUserManager(null);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());
        }

        public async Task CloseGameAsync(User user)
        {
            var game = await _gameRepository.GetGameAsync(user.GameID);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());

            await _gameRepository.CloseGameAsync(game.GameID);
        }
        private async Task UdateUserManager(GameLib.Models.Game game) =>
            await _onlineUserManager.UpdateUserGame(Context.ConnectionId, game?.GameID ?? Guid.Empty);
    }
}
