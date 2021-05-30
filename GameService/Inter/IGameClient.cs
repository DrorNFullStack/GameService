using GameLib.Models;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Inter
{
    public interface IGameClient
    {
        //sends the full lobby
        Task ReciveFullLobbyAsync(IEnumerable<User> lobby);
        //sends the new user only
        Task UserJoinedLobbyAsync(User user);
        //sends the disconnected user only
        Task UserLeftLobbyAsync(User user);
        Task GameRequested(string senderUsername);
        Task ReceiveGameView(GameView gameView);
        Task ReceiveTurn(Turn turn);
    }
}
