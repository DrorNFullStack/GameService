using System.Collections.Generic;

namespace GameLib.Models
{
    public class Turn
    {
        public IEnumerable<GameAction> PossibleActions { get; set; }
        public IEnumerable<DiceResult> DiceResults { get; set; }
        public IEnumerable<Triangle> Board { get; set; }
    }
}
