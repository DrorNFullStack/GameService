using GameLib.Models;
using GameLib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameLib.Tests
{
    public class BackgammonActionValidatorTests
    {
        [Fact]
        public void IsValidatingCorrectly()
        {
            //Arrange
            var validator = new BackgammonActionValidator();
            var action = new GameAction
            {
                StartingPosition = 1,
                DestinationPosition = 2,
                RelevantRoll = new DiceResult
                {
                    WasUsed = false,
                    Roll = 1
                }
            };
            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            var player = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.AntiClockWise,
                Name = "Tomer",
                RemainingActions = 2,
                IsDouble = false,
                DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                }
            };

            //Act
            var res = validator.Validate(action, board, player);

            //Assert
            Assert.True(res);
        }
        [Theory]
        [InlineData(1,2)]
        [InlineData(12,15)]
        public void IsValidatingCorrectlyTrue(int start, int dest)
        {
            //Arrange
            var validator = new BackgammonActionValidator();
            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = new DiceResult
                {
                    WasUsed = false,
                    Roll = Math.Abs(dest - start)
                }
            };
            var player = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.AntiClockWise,
                Name = "Tomer",
                RemainingActions = 2,
                IsDouble = false,
                DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                }
            };

            //Act
            var res = validator.Validate(action, board, player);

            //Assert
            Assert.True(res);
        }

        [Theory]
        [InlineData(1, 2, 2, false)]
        public void IsValidatingCorrectlyFalse(int start, int dest, int roll, bool wasRollUsed)
        {
            //Arrange
            var validator = new BackgammonActionValidator();
            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = new DiceResult
                {
                    WasUsed = wasRollUsed,
                    Roll = roll
                }
            };
            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            var player = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.AntiClockWise,
                Name = "Tomer",
                RemainingActions = 2,
                IsDouble = false,
                DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                }
            };

            //Act
            var res = validator.Validate(action, board, player);

            //Assert
            Assert.False(res);
        }
        [Fact]
        public void IsValidatingCorrectlyNotAllow()
        {
            //Arrange
            var validator = new BackgammonActionValidator();
            var action = new GameAction
            {
                StartingPosition = 1,
                DestinationPosition = 2,
                RelevantRoll = new DiceResult
                {
                    WasUsed = false,
                    Roll = 1
                }
            };
            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            var player = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.AntiClockWise,
                Name = "Tomer",
                RemainingActions = 2,
                IsDouble = false,
                DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                }
            };

            //Act
            var res = validator.Validate(action, board, player);

            //Assert
            Assert.True(res);
        }
    }
}
