using GameLib.Models;
using System.Collections.Generic;

namespace GameLib.Services
{
    public class GamePieceFactory : IGamePieceFactory
    {
        public IEnumerable<GamePiece> Create(int amountOfPieces, DirectionEnum? direction)
        {
            for (int i = 0; i < amountOfPieces; i++)
            {
                yield return new GamePiece { ControlledBy = direction ??DirectionEnum.ClockWise };
            }
        }
    }
}
