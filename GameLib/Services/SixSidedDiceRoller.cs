using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Services
{
    public class SixSidedDiceRoller : IDiceRoller<DiceResult>
    {
        const int sides = 6;
        private readonly Random random;
        public SixSidedDiceRoller()
        {
            random = new Random(DateTime.Now.Millisecond);
        }
        public DiceResult Roll() => new DiceResult
        {
            Roll = random.Next(1, sides + 1)
        };

        public IEnumerable<DiceResult> Roll(int amount)
        {
            for (int i = 0; i < amount; i++)
                yield return Roll();
        }
    }
}
