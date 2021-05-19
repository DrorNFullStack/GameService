using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Models
{
    public class Bar
    {
        public List<GamePiece> Pieces { get; set; }
        public bool IsEmpty => Pieces.Count == 0;
    }
}
