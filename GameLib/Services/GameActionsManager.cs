using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Services
{
    class GameActionsManager : IGameActionsManager
    {
        private readonly IBackgammonActionValidator backgammonActionValidator;

        public GameActionsManager(IBackgammonActionValidator backgammonActionValidator)
        {
            this.backgammonActionValidator = backgammonActionValidator;
        }

        public IEnumerable<GameAction> Act(GameAction action, Dictionary<int, Triangle> board, Player player)
        {
            bool isValidMove = backgammonActionValidator.Validate(action, board, player.Color);

            if (isValidMove == false)
                //illegal move
                return null;


            //move is legal
            board[action.DestinationPosition].GamePieces.AddLast(board[action.StartingPosition].GamePieces.Last);
            board[action.StartingPosition].GamePieces.RemoveLast();
            player.RemainingActions--;

            if (player.RemainingActions > 0) return GetActions(board, player, player.DiceResults);

            else return null;
        }

        public IEnumerable<GameAction> GetActions(Dictionary<int, Triangle> board, Player player, IEnumerable<DiceResult> diceResults)
        {
            var gameActions = new List<GameAction>();
            foreach (var triangle in board)
            {
                //if this triangle is controlled by the players color
                if (triangle.Value.GamePieces.First.Value.Color == player.Color)
                {
                    //go through the given rolls
                    foreach (var dr in diceResults)
                    {
                        //check the roll is still available
                        if (dr.WasUsed) continue;
                        var dest = triangle.Key + dr.Roll; //one player goes forward the other will go backwards?
                        //validate dest for dr
                        if (triangle.Key + dr.Roll > board.Count)
                        {
                            //roll will result in a higher than exists board size
                        }
                        if (board[triangle.Key + dr.Roll].GamePieces.Last.Value.Color == player.Color)
                        {
                            //dest will reach same color
                        }
                        else if (board[triangle.Key + dr.Roll].GamePieces.Count == 1)
                        {
                            //dest will reach other color but can eat
                        }
                        else
                        {
                            //dest will reach other color but can't eat
                        }
                        gameActions.Add(new GameAction
                        {
                            StartingPosition = triangle.Key,
                            DestinationPosition = dest
                        });

                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}
