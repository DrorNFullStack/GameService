using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Models
{
    public class GameBoard
    {
        public const int MaxPicesPerPlayer = 15;
        public Dictionary<int, Triangle> Triangles { get; set; }
        public Bar Bar { get; set; }
        public List<GamePiece> SafePieces { get; set; }
    }
}
