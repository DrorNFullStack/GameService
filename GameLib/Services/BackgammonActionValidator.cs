using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Services
{
    public class BackgammonActionValidator : IBackgammonActionValidator
    {
        public bool Validate(GameAction action, GameBoard board, Player player)
        {
            //no more actions
            if (player.RemainingActions < 1) return false;

            //validate direction
            switch (player.Direction)
            {
                case DirectionEnum.AntiClockWise:
                    if (action.DestinationPosition > action.StartingPosition && action.DestinationPosition != 0 && action.StartingPosition != -1) return false;
                    break;
                case DirectionEnum.ClockWise:
                    if (action.DestinationPosition < action.StartingPosition && action.DestinationPosition != 0 && action.StartingPosition != -1) return false;
                    break;
            }
            //if player has gp on bar he can't move from triangles
            if (action.StartingPosition > 0 && board.Bar.PiecesDirection.Equals(player.Direction)) return false;

            //if safty
            if (action.DestinationPosition == 0)
            {
                var safety = player.Direction == DirectionEnum.ClockWise ? 25 : 0;
                var neededRoll = Math.Abs(safety - action.StartingPosition);
                var isReady = ValidateReadyForSaftey(board, player.Direction, (i)=> 
                {
                    if(neededRoll < action.RelevantRoll.Roll)
                        return player.Direction.Equals(DirectionEnum.ClockWise) ? i < action.StartingPosition : i > action.StartingPosition &&
                             (board.Triangles[i].PiecesDirection?.Equals(player.Direction) ?? false);
                    return false;
                });
                if (isReady == false) return false;
                return true;
            }

            //if wants to leave Bar
            if (action.StartingPosition < 0)
            {
                //check if outside of HomeBase
                if (player.Direction > 0)
                {
                    if (action.DestinationPosition >= 7 || action.DestinationPosition <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (action.DestinationPosition >= 25 || action.DestinationPosition <= 18)
                    {
                        return false;
                    }
                }
            }

            //more than one, not player's color
            if (board.Triangles[action.DestinationPosition].GamePieces.Count > 1 &&
                !board.Triangles[action.DestinationPosition].PiecesDirection.Equals(player.Direction)) return false;


            return true;
        }
        private bool ValidateReadyForSaftey(GameBoard board, DirectionEnum direction, Predicate<int> extraCheck)
        {
            var safety = direction == DirectionEnum.ClockWise ? 25 : 0;
            var start = safety - (int)direction;
            var end = Math.Abs(safety - 6);
            var cnt = board.SafePieces.Count(gp => gp.ControlledBy.Equals(direction));
            for (int i = start; direction.Equals(DirectionEnum.AntiClockWise)? i <= end :i>=end; i -= (int)direction)
            {
                //add the pices to the counter
                cnt += board.Triangles[i].PiecesDirection.Equals(direction) ? board.Triangles[i].GamePieces.Count : 0;
                if (extraCheck(i)) return false;
            }
            return cnt == GameBoard.MaxPicesPerPlayer;
        }
    }
}
