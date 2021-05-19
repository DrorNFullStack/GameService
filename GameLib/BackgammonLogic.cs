using GameLib.Models;
using GameLib.Services;
using System.Collections.Generic;
using System.Linq;

namespace GameLib
{
    internal class BackgammonLogic
    {
        //public Queue<GameAction> TurnHistory { get; set; }

        private readonly IDiceRoller<DiceResult> diceRoller;
        private readonly ITurnKeeper turnKeeper;
        private readonly IGameActionsManager gameActionsManager;
        private readonly IGamePieceFactory gamePieceFactory;

        public BackgammonLogic(IDiceRoller<DiceResult> diceRoller, ITurnKeeper turnKeeper, IGameActionsManager gameActionsManager)
        {
            this.diceRoller = diceRoller;
            this.turnKeeper = turnKeeper;
            this.gameActionsManager = gameActionsManager;
        }

        public Dictionary<int,Triangle> Board { get; set; }
        public void StartGame(Player player1, Player player2)
        {
            Board = new Dictionary<int, Triangle>();
            for (int i = 1; i <= 24; i++)
            {
                var pieceR = new GamePiece { Color = "red" };
                var pieceB = new GamePiece { Color = "black" };
            }

        }
        public IEnumerable<GameAction> PerformAction(GameAction action, Player player)
        {
            var active = turnKeeper.GetActivePlayer();
            if (!active.Equals(player))
            {
                return null;
            }

            //preform the action
            var availableActions = gameActionsManager.Act(action, Board, player);

            
            //action completed
            if (player.RemainingActions < 1)
                turnKeeper.EndTurn(player);

            return availableActions;
        }
        public Turn StartTurn(Player player)
        {
            if (!player.Equals(turnKeeper.GetActivePlayer()))
                return null;

            //Get Dice Res
            var diceResults = diceRoller.Roll(2);

            //Get Possible Actions
            var possibleActions = gameActionsManager.GetActions(Board, player, diceResults);

            var boardView = Board.Values.Select(triangle => triangle);
            return new Turn
            {
                PossibleActions = possibleActions,
                DiceResults = diceResults,
                Board = boardView
            };
        }
    }
}
