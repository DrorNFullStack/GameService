using GameLib.Models;
using GameLib.Services;
using System.Collections.Generic;
using System.Linq;

namespace GameLib
{
    public class BackgammonLogic
    {
        //public Queue<GameAction> TurnHistory { get; set; }

        private readonly IDiceRoller<DiceResult> diceRoller;
        private readonly ITurnKeeper turnKeeper;
        private readonly IGameActionsManager gameActionsManager;
        private readonly IGameBoardFactory gameBoardFactory;

        public BackgammonLogic(IDiceRoller<DiceResult> diceRoller, ITurnKeeper turnKeeper, IGameActionsManager gameActionsManager, IGameBoardFactory gameBoardFactory)
        {
            this.diceRoller = diceRoller;
            this.turnKeeper = turnKeeper;
            this.gameActionsManager = gameActionsManager;
            this.gameBoardFactory = gameBoardFactory;
        }
        public GameBoard Board { get; set; }
        /// <summary>
        /// Start a new Game, with the two players.
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns>The starting player</returns>
        public Player StartGame(Player player1, Player player2)
        {
            //Generate board
            Board = gameBoardFactory.Create();

            //add players to turn queue
            turnKeeper.AddPlayer(player1);
            turnKeeper.AddPlayer(player2);

            //return who's starting
            return turnKeeper.GetActivePlayer();
        }

        //start the players turn
        public Turn StartTurn(Player player)
        {
            if (!player.Equals(turnKeeper.GetActivePlayer()))
                return null;

            //Get Dice Res
            player.DiceResults = diceRoller.Roll(2);

            //Get Possible Actions
            var possibleActions = gameActionsManager.GetActions(Board, player);

            var boardView = Board.Triangles.Values;
            return new Turn
            {
                PossibleActions = possibleActions,
                DiceResults = player.DiceResults,
                Board = boardView
            };
        }

        public Turn PerformAction(GameAction action, Player player)
        {
            var active = turnKeeper.GetActivePlayer();
            if (!active.Equals(player))
            {
                return null;
            }

            //preform the action
            var success = gameActionsManager.Act(action, Board, player, out IEnumerable<GameAction> availableActions);

            if (!success)
            {
                return null;
            }

            //action completed
            if (player.RemainingActions < 1)
                turnKeeper.EndTurn(player);

            return new Turn
            {
                PossibleActions = availableActions,
                DiceResults = player.DiceResults,
                Board = Board.Triangles.Values
            };
        }
    }
}
