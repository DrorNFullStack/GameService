using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Services
{
    class GameBoardFactory : IGameBoardFactory
    {
        private readonly IGamePieceFactory gamePieceFactory;

        public GameBoardFactory(IGamePieceFactory gamePieceFactory)
        {
            this.gamePieceFactory = gamePieceFactory;
        }

        public GameBoard Create()
        {
            var board = new Dictionary<int, Triangle>();
            int length = 24;
            for (int i = 1; i <= length / 2; i++)
            {

                int position = i;
                int amount = 0;
                string color = null;

                switch (i)
                {
                    case 0: break;
                    case 1:
                        amount = 2;
                        color = "Red";
                        break;
                    case 6:
                        amount = 5;
                        color = "Black";
                        break;
                    case 8:
                        amount = 3;
                        color = "Black";
                        break;
                    case 12:
                        amount = 5;
                        color = "Red";
                        break;
                }

                //Player 1 prespective flow
                var pieces = gamePieceFactory.Create(amount, color);
                var triangle = new Triangle { Position = position, GamePieces = new LinkedList<GamePiece>(pieces) };
                board.Add(triangle.Position, triangle);

                //Player 2 prespective flow
                position = length - i + 1;
                color = color == "Red" ? "Black" : "Red";
                pieces = gamePieceFactory.Create(amount, color);
                triangle = new Triangle { Position = position, GamePieces = new LinkedList<GamePiece>(pieces) };
                board.Add(triangle.Position, triangle);
            }
            return new GameBoard 
            {
                Bar = new Bar(),
                SafePieces = new List<GamePiece>(),
                Triangles = board
            };
        }
    }
}
