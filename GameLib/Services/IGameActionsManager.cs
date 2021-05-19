using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    public interface IGameActionsManager
    {
        bool Act(GameAction action, GameBoard board, Player player, out IEnumerable<GameAction> gameActions);
        IEnumerable<GameAction> GetActions(GameBoard board, Player player);
    }
}