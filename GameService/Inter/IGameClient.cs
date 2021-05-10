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
        Task GameCreatedAsync(Game game); 
        Task GameRequested(User user); /*User A (Sender) sends a request to user B (receiver) to start a game. User B will see a popup, saying
        they have been invited and will be prompted to either say yes or no. */
        Task InvitedToGameAsync(string gameID); /*After the game is created by the host, send invitations to those interested. RSVP please.*/
        Task ErrorHandling(string msg);
    }
}
