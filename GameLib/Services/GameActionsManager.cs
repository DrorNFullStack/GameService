using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Services
{
    class GameActionsManager : IGameActionsManager
    {
        public IEnumerable<GameAction> Act(GameAction action, Dictionary<int, Triangle> board, Player player)
        {
            if (player.Color != board[action.StartingPosition].GamePieces.First?.Value.Color 
                    || (player.Color != board[action.DestinationPosition].GamePieces.First?.Value.Color
                         && board[action.DestinationPosition].GamePieces.Count > 1))
                    //illegal move
                    return null;


            var usedRoll = player.DiceResults.FirstOrDefault(
                (dr) => dr.Roll.Equals(Math.Abs(action.DestinationPosition - action.StartingPosition
                )));

            if (usedRoll is null || usedRoll.WasUsed) return null;

            //move is legal
            board[action.DestinationPosition].GamePieces.AddLast(board[action.StartingPosition].GamePieces.Last);
            board[action.StartingPosition].GamePieces.RemoveLast();
            player.RemainingActions--;

            
           

            if ( player.RemainingActions > 0 ) return GetActions(board, player, player.DiceResults)
        }

        public IEnumerable<GameAction> GetActions(Dictionary<int, Triangle> board, Player player, IEnumerable<DiceResult> diceResults)
        {
            throw new NotImplementedException();
        }
    }
}
