using GameLib.Models;
using GameLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GameLib.Tests
{
    public class BackgammonActionValidatorTests
    {
        private (GameBoard board, Player player, BackgammonActionValidator validator) GetBaseSUT()
        {
            var validator = new BackgammonActionValidator();

            var board = new GameBoardFactory(new GamePieceFactory()).Create();
            var player = new Player
            {
                Name = "Tomer",
                IsDouble = false,
                DiceResults = new List<DiceResult>()
            };
            return (board, player, validator);
        }

        
        [Theory]
        [InlineData(20, 21, 1, false, DirectionEnum.ClockWise, 2)]
        //[InlineData(15, 10, 5, false, DirectionEnum.ClockWise, 2)]
        [InlineData(1, 7, 6, false, DirectionEnum.ClockWise, 2)]
        [InlineData(3, 2, 1, false, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(10, 15, 5, false, DirectionEnum.AntiClockWise, 2)]
        [InlineData(24, 20, 4, false, DirectionEnum.AntiClockWise, 2)]


        //Try and move when you can, both directions
        [InlineData(12,14,1,false,DirectionEnum.ClockWise, 1)]
        [InlineData(13,11,1,false,DirectionEnum.AntiClockWise, 1)]

        //
        public void IsValidatedWhenValid(int start, int dest, int roll, bool wasRollUsed, DirectionEnum direction, int remActions)
        {
            var SUT = GetBaseSUT();
            //Arrange

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

            SUT.player.Direction = direction;
            SUT.player.RemainingActions = remActions;
            SUT.player.DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                };

            //Act
            var res = SUT.validator.Validate(action, SUT.board, SUT.player);

            //Assert
            Assert.True(res);
        }

        [Theory]
        [InlineData(1, 2, 1, false, DirectionEnum.AntiClockWise, 2)]//going the wrong way
        [InlineData(3, 5, 2, false, DirectionEnum.AntiClockWise, 2)]
        [InlineData(4, 7, 3, false, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(10, 5, 5, false, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(8, 3, 4, false, DirectionEnum.AntiClockWise, 2)]//moving more than available
        //[InlineData(11, 4, 6, false, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(10, 4, 6, true, DirectionEnum.AntiClockWise, 2)]//Roll used
        //[InlineData(24, 20, 4, true, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(7, 0, 6, false, DirectionEnum.AntiClockWise, 2)]//Trying to get to safe when can't
        //[InlineData(9, 0, 6, false, DirectionEnum.AntiClockWise, 2)]
        //[InlineData(6, 0, 6, false, DirectionEnum.AntiClockWise, 2)]//Board still filled, can't go safe
        //[InlineData(23, 0, 2, false, DirectionEnum.ClockWise, 2)]
        [InlineData(1, 4, 3, true, DirectionEnum.ClockWise, 0)] //No more actions. Roll marked as used.
        [InlineData(1, 3, 2, false, DirectionEnum.ClockWise, 0)] //No more actions. Roll marked as unused. 
        //[InlineData(3, 6, 3, false, DirectionEnum.ClockWise, 2)] //Going wrong way black
        //[InlineData(15, 20, 5, false, DirectionEnum.ClockWise, 2)]


        //Try and move when you can't, both directions (not available triagle)
        [InlineData(1, 6, 5, false, DirectionEnum.ClockWise, 2)]
        [InlineData(13, 1, 1, false, DirectionEnum.AntiClockWise, 2)]
        public void IsInvalidatedWhenInvalid(int start, int dest, int roll, bool wasRollUsed, DirectionEnum direction, int remActions)
        {
            var SUT = GetBaseSUT();
            //Arrange

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

            SUT.player.Direction = direction;
            SUT.player.RemainingActions = remActions;
            SUT.player.DiceResults = new DiceResult[]
                {
                    action.RelevantRoll,
                    new DiceResult
                    {
                        Roll = 3,
                        WasUsed = false
                    }
                };

            //Act
            var res = SUT.validator.Validate(action, SUT.board, SUT.player);

            //Assert
            Assert.False(res);
        }

        [Theory]
        [InlineData(24, 20, 4, DirectionEnum.AntiClockWise)] //Red eating
        [InlineData(16, 15, 1, DirectionEnum.AntiClockWise)]
        [InlineData(1, 5, 4, DirectionEnum.ClockWise)] //Black eating
        [InlineData(7, 10, 3, DirectionEnum.ClockWise)]
        public void CanEat(int start, int dest, int roll, DirectionEnum direction)
        {
            //arrange
            var (board, player, validator) = GetBaseSUT();

            board.Triangles[20].GamePieces.AddLast(new GamePiece { ControlledBy = DirectionEnum.ClockWise});
            board.Triangles[15].GamePieces.AddLast(new GamePiece { ControlledBy = DirectionEnum.ClockWise });
            board.Triangles[10].GamePieces.AddLast(new GamePiece { ControlledBy = DirectionEnum.AntiClockWise });
            board.Triangles[5].GamePieces.AddLast(new GamePiece { ControlledBy = DirectionEnum.AntiClockWise });

            player.RemainingActions = 2;
            player.Direction = direction;
            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = new DiceResult
                {
                    WasUsed = false,
                    Roll = roll
                }
            };
            //act
            var res = validator.Validate(action, board, player);
            //assert
            Assert.True(res);
        }

        [Theory]
        [InlineData(6,0,6,DirectionEnum.AntiClockWise)]
        [InlineData(19,0,6,DirectionEnum.ClockWise)]
        public void CanReachSafety(int start, int dest, int roll, DirectionEnum direction)
        {
            var (board, player, validator) = GetBaseSUT();
            player.Direction = direction;
            player.RemainingActions = 1;
            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = new DiceResult
                {
                    Roll = roll
                }
            };

            //clear all pieces
            board.Triangles[1].GamePieces.Clear();
            board.Triangles[8].GamePieces.Clear();
            board.Triangles[12].GamePieces.Clear();
            board.Triangles[13].GamePieces.Clear();
            board.Triangles[17].GamePieces.Clear();
            board.Triangles[24].GamePieces.Clear();

            //remains 6 anticlockwise and 19 clockwise
            //add 10 safe pieces to the player
            board.SafePieces.AddRange(Enumerable.Repeat(new GamePiece { ControlledBy = direction }, 10));
           var issuccess = validator.Validate(action, board, player);

            //must succeed
            Assert.True(issuccess);
        }

        [Theory]
        [InlineData(2,0,6,6,DirectionEnum.AntiClockWise)] //blocked close
        [InlineData(23,0,6,19,DirectionEnum.ClockWise)]
        [InlineData(2, 0, 6, 24, DirectionEnum.AntiClockWise)] //blocked far
        [InlineData(23, 0, 6, 1, DirectionEnum.ClockWise)]

        [InlineData(23,0,2,18,DirectionEnum.ClockWise)] //exact roll but blocked by pieces out side home base
        [InlineData(2,0,2,7,DirectionEnum.ClockWise)] //exact roll but blocked by pieces out side home base
        public void CanNOTReachSafty(int start, int dest, int roll,int blockedPosition, DirectionEnum direction)
        {
            var (board, player, validator) = GetBaseSUT();
            player.Direction = direction;
            player.RemainingActions = 1;
            var action = new GameAction
            {
                StartingPosition = start,
                DestinationPosition = dest,
                RelevantRoll = new DiceResult
                {
                    Roll = roll
                }
            };

            //clear all pieces
            board.Triangles[1].GamePieces.Clear();
            board.Triangles[6].GamePieces.Clear();
            board.Triangles[8].GamePieces.Clear();
            board.Triangles[12].GamePieces.Clear();
            board.Triangles[13].GamePieces.Clear();
            board.Triangles[17].GamePieces.Clear();
            board.Triangles[19].GamePieces.Clear();
            board.Triangles[24].GamePieces.Clear();

            //Add Blocking Pieces
            board.Triangles[blockedPosition].GamePieces.AddLast(new GamePiece { ControlledBy = direction});


            //Add pieces to try and get to saftey
            board.Triangles[start].GamePieces.AddLast(new GamePiece { ControlledBy = direction });
            //remains 6 anticlockwise and 19 clockwise

            var issuccess = validator.Validate(action, board, player);

            //must succeed
            Assert.False(issuccess);
        }
    }
}


/*  NEDDED TESTS
Try and move when you can, both directions
Try and move when you cant't, both directions
try and eat an enemy when you can both directions
try and eat an enemy when you can't both directions
try and go to safty when you can both directions
try and go to safty when you can't both directions
 */