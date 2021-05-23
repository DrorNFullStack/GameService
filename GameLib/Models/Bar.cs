using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Models
{
    public class Bar
    {
        public List<GamePiece> Pieces { get; set; } = new List<GamePiece>();
        public bool IsEmpty => Pieces.Count == 0;
        public string PiecesColor => Pieces?.FirstOrDefault()?.Color ?? string.Empty;
    }
}
