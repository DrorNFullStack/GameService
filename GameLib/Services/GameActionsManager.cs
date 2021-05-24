using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Services
{
    public class GameActionsManager : IGameActionsManager
    {
        private readonly IBackgammonActionValidator backgammonActionValidator;

        public GameActionsManager(IBackgammonActionValidator backgammonActionValidator)
        {
            this.backgammonActionValidator = backgammonActionValidator;
        }

        public bool Act(GameAction action, GameBoard board, Player player, out IEnumerable<GameAction> gameActions)
        {
            gameActions = null;
            bool isValidMove = backgammonActionValidator.Validate(action, board, player);

            if (isValidMove == false)
                //illegal move
                return false;
            GamePiece gamePiece = null;
            //Starting from Bar 
            if (action.StartingPosition < 0)
            {
                gamePiece = board.Bar.Pieces.FirstOrDefault(gp => gp.ControlledBy.Equals(player.Direction));
                board.Bar.Pieces.Remove(gamePiece);
            }
            else
            {
                //Starting from a Triangle
                gamePiece = board.Triangles[action.StartingPosition].GamePieces.Last.Value;
                board.Triangles[action.StartingPosition].GamePieces.RemoveLast();
            }
            //Going Safe
            if (action.DestinationPosition == 0)
            {
                board.SafePieces.Add(gamePiece);
            }
            //going to a triangle (validator validates if can eat)
            //going to home / empty triangle
            else
            {
                //if not empty triangle or not my direction
                if (!board.Triangles[action.DestinationPosition].PiecesDirection?.Equals(player.Direction) ?? false)
                {
                    //remove the dest piece
                    var deadPiece = board.Triangles[action.DestinationPosition].GamePieces.Last.Value;
                    board.Triangles[action.DestinationPosition].GamePieces.RemoveLast();
                    //add it to the bar
                    board.Bar.Pieces.Add(deadPiece);
                }
                //add the piece to the destenation triangle
                board.Triangles[action.DestinationPosition].GamePieces.AddLast(gamePiece);
            }
            //consume an action
            player.RemainingActions--;

            //mark die as used if not a Double
            if (player.IsDouble == false)
                player.DiceResults.First(dr => dr.Roll == action.RelevantRoll.Roll).WasUsed = true;

            gameActions = GetActions(board, player);
            return true;
        }

        public IEnumerable<GameAction> GetActions(GameBoard board, Player player)
        {
            var possibleActions = new List<GameAction>();
            var haveInBar = board.Bar.Pieces.Any(gp => gp.ControlledBy.Equals(player.Direction));
            foreach (var diceResult in player.DiceResults)
            {
                if (haveInBar)
                {
                    GameAction action = new GameAction
                    {
                        RelevantRoll = diceResult,
                        StartingPosition = 0,
                        DestinationPosition = player.Direction > 0 ? diceResult.Roll : 25 - diceResult.Roll
                    };
                    var validation = backgammonActionValidator.Validate(action, board, player);
                    if (validation) possibleActions.Add(action);
                }
                else
                {
                    foreach (var triangle in board.Triangles.Where(t => t.Value.PiecesDirection.Equals(player.Direction)))
                    {
                        GameAction action = new GameAction
                        {
                            RelevantRoll = diceResult,
                            StartingPosition = triangle.Value.Position,
                            DestinationPosition = player.Direction > 0 ? diceResult.Roll + triangle.Value.Position : triangle.Value.Position - diceResult.Roll 
                        };
                        var validation = backgammonActionValidator.Validate(action, board, player);
                        if (validation) possibleActions.Add(action);
                    }
                }
            }
            return possibleActions;
        }
    }
}