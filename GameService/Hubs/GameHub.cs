using GameService.Inter;
using GameService.GameRepositories;
using GameService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        //Only a hub, connecting two sides
        private readonly IGameRepository _gameRepository;

        public GameHub(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override Task OnConnectedAsync() => base.OnConnectedAsync();
        public override Task OnDisconnectedAsync(Exception exception) => base.OnDisconnectedAsync(exception);

        public async Task CreateGameAsync(User user)
        {
            /*Create the game, have the creator join it first by adding him to the group. Second player joining will be handled in function
            JoinGameAsync below.*/

            var game = await _gameRepository.CreateGameAsync();

            user.GameID = game.GameID;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, user.GameID.ToString());
            
            await Clients.Group(user.GameID.ToString()).GameCreatedAsync(game);
        }

        public async Task JoinGameAsync(User user)
        {
            /*Find the game. Use the game ID to let the user into the game. Using the user.GameID in GetGameAsync 'cus we're changing the
            user's GameID according to the game he wanted to join. */

            var game = await _gameRepository.GetGameAsync(user.GameID);

            await Groups.AddToGroupAsync(Context.ConnectionId, game.GameID.ToString());
        }

        public async Task LeaveGameAsync(User user)
        {
            var game = await _gameRepository.GetGameAsync(user.GameID);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());
        }

        public async Task CloseGameAsync(User user)
        {
            //If both the users' game ID isn't the current game's ID, basically, both of them leaving, close the game. 

            /*In layman's terms: The hub needs only to close the game upon request, the request to close the game shall come once the last user
            has left it. The hub is only to open/close connections, not to handle logic as to when or why*/

            var game = await _gameRepository.GetGameAsync(user.GameID);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GameID.ToString());

            await _gameRepository.CloseGameAsync(game.GameID);
        }
    }
}
