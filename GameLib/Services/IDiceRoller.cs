using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    public interface IDiceRoller<T> where T: DiceResult
    {
        T Roll();
        IEnumerable<T> Roll(int amount);
    }
}
