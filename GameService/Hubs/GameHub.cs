using GameService.Inter;
using GameService.GameRepositories;
using GameService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.OnlineUserManager;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GameService.Hubs
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {
        private readonly IOnlineUserManager _onlineUserManager;
        public GameHub(IOnlineUserManager onlineUserManager)
        {
            this._onlineUserManager = onlineUserManager;
        }

        public override async Task OnConnectedAsync()
        {
            User user = new User { UserID = Context.ConnectionId, Username = GetNameFromClaims() };
            await _onlineUserManager.AddLiveUser(user);
            var excluded = await _onlineUserManager.GetUnavailbeUsers();
            excluded = excluded.Append(user);
            await Clients.AllExcept(excluded.Select(u=>u.UserID).ToList().AsReadOnly()).LobbyUpdatedAsync(user);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _onlineUserManager.RemoveLiveUser(Context.ConnectionId);
            var excluded = await _onlineUserManager.GetUnavailbeUsers();
            await Clients.AllExcept(excluded.Select(u=>u.UserID).ToList().AsReadOnly()).LobbyUpdatedAsync(user);
            await base.OnDisconnectedAsync(exception);
        }

        // Gets The lobby of online users (that are free to play with)
        public async Task GetLobbyAsync()
        {
           var lobby = await _onlineUserManager.GetAvailbeUsers();
           await Clients.Caller.ReciveFullLobbyAsync(lobby);
        }

        private string GetNameFromClaims() =>
            Context.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;

        //public async Task RequestGame(User receiver, User sender)
        //{
        //    await Clients.User(receiver.UserID).GameRequested(sender);
        //}

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
