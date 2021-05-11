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

        public BackgammonLogic(IDiceRoller<DiceResult> diceRoller, ITurnKeeper turnKeeper)
        {
            this.diceRoller = diceRoller;
            this.turnKeeper = turnKeeper;
        }

        public Dictionary<int,Triangle> Board { get; set; }

        public bool PerformAction(GameAction action, Player player)
        {
            var active = turnKeeper.GetActivePlayer();
            if (!active.Equals(player))
            {
                return false;
            }


            //action completed
            turnKeeper.EndTurn(player);
            return true;
        }
        public Turn StartTurn(Player player)
        {
            if (!player.Equals(turnKeeper.GetActivePlayer()))
                return null;
            //Get Dice Res
            var diceResults = diceRoller.Roll(2);
            //Get Possible Actions
            var possibleActions = "";

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
