using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Models
{
    public class GameView
    {
        public IEnumerable<Triangle> Triangles { get; internal set; }
        public IEnumerable<GamePiece> Bar { get; internal set; }
        public IEnumerable<GamePiece> SafePieces { get; internal set; }
    }
}
