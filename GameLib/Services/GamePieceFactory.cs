using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    class GamePieceFactory : IGamePieceFactory
    {
        public IEnumerable<GamePiece> Create(int amountOfPieces, string color)
        {
            for (int i = 0; i < amountOfPieces; i++)
            {
                yield return new GamePiece { Color = color };
            }
        }
    }
}
