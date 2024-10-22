﻿using System.Collections.Generic;

namespace GameLib.Models
{
    public class Player
    {
        public int Score { get; set; }
        public string PlayerID { get; set; }
        public string Name { get; set; }
        public int RemainingActions { get; set; }
        public IEnumerable<DiceResult> DiceResults { get; set; }
        public bool IsDouble { get; set; }
        public DirectionEnum Direction { get; set; }
    }
}