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
                Direction = DirectionEnum.ClockWise,
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

            var mockValidator = new Mock<IBackgammonActionValidator>();
            mockValidator.Setup<bool>(bl => bl.Validate(action, board, player)).Returns(true);
            var gameActionManager = new GameActionsManager(mockValidator.Object);

            //Act
            var isScuccess = gameActionManager.Act(action, board, player, out IEnumerable<GameAction> res);

            //Assert
            Assert.True(isScuccess);
            Assert.True(!board.Bar.IsEmpty);
            Assert.True(board.Triangles[action.DestinationPosition].GamePieces.Last.Value.ControlledBy.Equals(player.Direction));
        }

        [Theory]
        [InlineData(1,4,3,DirectionEnum.ClockWise)]
        [InlineData(24,20,4,DirectionEnum.AntiClockWise)]
        public void MovePieceTheory(int start, int dest, int roll, DirectionEnum direction)
        {
            //Arrange
            var board = new GameBoardFactory(new GamePieceFactory()).Create();

            var player = new Player
            {
                Direction = direction,
                RemainingActions = 2,
                DiceResults = new DiceResult[]
                {
                    new DiceResult
                    {
                        Roll = roll,
                        WasUsed = false
                    }
                }
            };

            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = player.DiceResults.First()
            };

            var mockValidator = new Mock<IBackgammonActionValidator>();
            mockValidator.Setup<bool>(bl => bl.Validate(action, board, player)).Returns(true);
            var gameActionManager = new GameActionsManager(mockValidator.Object);
            int expectedCount = board.Triangles[action.StartingPosition].GamePieces.Count - 1;

            //act
            var isSuccess = gameActionManager.Act(action, board, player,out IEnumerable<GameAction> res);

            Assert.True(board.Triangles[action.StartingPosition].GamePieces.Count.Equals(expectedCount));
            Assert.True(board.Triangles[action.DestinationPosition].GamePieces.Last.Value.ControlledBy.Equals(player.Direction));
        }
    }
}
