﻿using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Services
{
   public class GameBoardFactory : IGameBoardFactory
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
                DirectionEnum? direction = null;

                switch (i)
                {
                    case 0: break;
                    case 1:
                        amount = 2;
                        direction = DirectionEnum.ClockWise;
                        break;
                    case 6:
                        amount = 5;
                        direction = DirectionEnum.AntiClockWise;
                        break;
                    case 8:
                        amount = 3;
                        direction = DirectionEnum.AntiClockWise;
                        break;
                    case 12:
                        amount = 5;
                        direction = DirectionEnum.ClockWise;
                        break;
                }

                //Player 1 prespective flow
                var pieces = gamePieceFactory.Create(amount, direction);
                var triangle = new Triangle { Position = position, GamePieces = new LinkedList<GamePiece>(pieces) };
                board.Add(triangle.Position, triangle);

                //Player 2 prespective flow
                position = length - i + 1;
                direction = direction == DirectionEnum.AntiClockWise ? DirectionEnum.ClockWise : DirectionEnum.AntiClockWise;
                pieces = gamePieceFactory.Create(amount, direction);
                triangle = new Triangle { Position = position, GamePieces = new LinkedList<GamePiece>(pieces) };
                board.Add(triangle.Position, triangle);
            }
            return new GameBoard 
            {
                Bar = new Bar 
                {
                    Pieces = new List<GamePiece>()
                },
                SafePieces = new List<GamePiece>(),
                Triangles = board
            };
        }
    }
}
