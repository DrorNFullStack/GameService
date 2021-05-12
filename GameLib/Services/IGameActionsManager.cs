using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    internal interface IGameActionsManager
    {
        IEnumerable<GameAction> Act(GameAction action, Dictionary<int, Triangle> board, Player player);
        IEnumerable<GameAction> GetActions(Dictionary<int, Triangle> board, Player player, IEnumerable<DiceResult> diceResults);
    }
}