using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    internal interface IGamePieceFactory
    {
        IEnumerable<GamePiece> Create(int amountOfPieces, string color);
    }
}