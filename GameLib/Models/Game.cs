using GameLib.Inter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Models
{
    public class Game 
    {
        private readonly IBackgammonLogic _backgammonLogic;

        public Player _playerBlue, _playerRed;

        public int[,] _boardPieces;
        
        public Guid GameID { get; set; }

        public Game(IBackgammonLogic backgammonLogic)
        {
            backgammonLogic = _backgammonLogic;
        }
        
        public int[] RollTheDice(Player roller)
        {
            int[] result = _backgammonLogic.DiceRoller();

            return result;
        }
    }
}
