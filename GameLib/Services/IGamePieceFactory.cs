using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    public interface IGamePieceFactory
    {
        IEnumerable<GamePiece> Create(int amountOfPieces, string color);
    }
}