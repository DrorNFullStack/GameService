using System.Collections.Generic;

namespace GameLib.Models
{
    public class Triangle
    {
        public int Position { get; set; }
        public LinkedList<GamePiece> GamePieces { get; set; }
        public string PiecesColor => GamePieces?.Last?.Value?.Color ?? string.Empty;
    }
}
