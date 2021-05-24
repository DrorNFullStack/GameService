using System.Collections.Generic;

namespace GameLib.Models
{
    public class Triangle
    {
        public int Position { get; set; }
        public LinkedList<GamePiece> GamePieces { get; set; }
        public DirectionEnum? PiecesDirection => GamePieces?.Last?.Value.ControlledBy;
        public bool IsEmpty => GamePieces?.Count > 0;
        public bool IsHome => GamePieces?.Count > 1;
    }
}
