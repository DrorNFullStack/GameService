using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<DiceResult> drList = new List<DiceResult>();
            int[] array = new int[6];

            var oldRoll = new DiceResult();

            for (int i = 0; i < amount; i++)
            {
                drList.Add(Roll());

            }
            drList.ForEach(a => array[a.Roll-1] = drList.Count(z => z.Roll == a.Roll));

            return drList;
           
        }
    }
}
