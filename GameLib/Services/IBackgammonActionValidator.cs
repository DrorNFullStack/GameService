using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    public interface IBackgammonActionValidator
    {
        bool Validate(GameAction action, GameBoard board, Player player);
    }
}