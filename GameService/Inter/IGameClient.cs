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
        Task GameCreatedAsync(); 
        Task GameRequested(User user); 
        Task InvitedToGameAsync(string gameID);
        Task ErrorHandling(string msg);
        Task LobbyUpdatedAsync(User user);
        Task ReciveFullLobbyAsync(IEnumerable<User> lobby);
    }
}
