using GameLib.Models;
using GameLib.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GameLib.Tests
{
    public class GameActionsManagerTests
    {
        [Fact]
        public void EatPieceTrue()
        {
            //Arrange
            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            board.Triangles[24].GamePieces.RemoveLast();

            var player = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.AntiClockWise,
                RemainingActions = 2,
                DiceResults = new DiceResult[]
                {
                    new DiceResult
                    {
                        Roll = 5,
                        WasUsed = false
                    } 
                }
            };

            var action = new GameAction
            {
                StartingPosition = 19,
                DestinationPosition = 24,
                RelevantRoll = player.DiceResults.First()
            };

            var mockValidatort = new Mock<IBackgammonActionValidator>();
            mockValidatort.Setup<bool>(bl => bl.Validate(action, board, player)).Returns(true);
            var gameActionManager = new GameActionsManager(mockValidatort.Object);



            //Act
            var isScuccess = gameActionManager.Act(action, board, player, out IEnumerable<GameAction> res);

            Assert.True(isScuccess);
            Assert.True(!board.Bar.IsEmpty);

            Assert.True(board.Triangles[action.DestinationPosition].GamePieces.Last.Value.Color.Equals(player.Color));
        }
    }
}
