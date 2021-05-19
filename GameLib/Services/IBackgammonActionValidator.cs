using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    internal interface IBackgammonActionValidator
    {
        bool Validate(GameAction action, GameBoard board, Player player);
    }
}