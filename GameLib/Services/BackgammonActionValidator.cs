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
                case DirectionEnum.ClockWise:
                    if (action.DestinationPosition - action.StartingPosition < 0) return false;
                    break;
                case DirectionEnum.AntiClockWise:
                    if (action.DestinationPosition - action.StartingPosition > 0) return false;
                    break;
            }
            //if player has gp on bar he can't move from triangles
            if (action.StartingPosition > 0 && board.Bar.PiecesColor.Equals(player.Color)) return false;

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
                !board.Triangles[action.DestinationPosition].PiecesColor.Equals(player.Color)) return false;

            if (action.DestinationPosition == 0)
            {
                var realDest = player.Direction == DirectionEnum.ClockWise ? action.StartingPosition + action.RelevantRoll.Roll : action.StartingPosition - action.RelevantRoll.Roll;
                if(realDest < 0 || realDest > 24)
                {
                    var actualRollPosition = player.Direction == DirectionEnum.ClockWise ? action.RelevantRoll.Roll : 25 - action.RelevantRoll.Roll;
                    if (actualRollPosition != action.StartingPosition && board.Triangles[actualRollPosition].GamePieces.Count > 0) return false;
                }
            }
            return true;
        }
    }
}
