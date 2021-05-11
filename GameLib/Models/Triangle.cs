using System.Collections.Generic;

namespace GameLib.Models
{
    public class Triangle
    {
        public int Position { get; set; }
        public IEnumerable<GamePiece> GamePieces { get; set; }
    }
}
