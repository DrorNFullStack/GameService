using GameLib.Models;
using GameLib.Services;
using System;
using Xunit;

namespace GameLib.Tests
{
    public class UnitTest
    {
        [Fact]
        public void GenerateBoardTest()
        {
            //Arrange
            var diceRoller = new SixSidedDiceRoller();
            var turnKeeper = new TurnKeeper();
            var vaidatator = new BackgammonActionValidator();
            var gameActionManager = new GameActionsManager(vaidatator);
            var gamePiecesFactory = new GamePieceFactory();
            var gameBoardFactory = new GameBoardFactory(gamePiecesFactory);
            var game = new BackgammonLogic(diceRoller, turnKeeper, gameActionManager, gameBoardFactory);
            var player1 = new Player
            {
                Color = "Red",
                Direction = DirectionEnum.ClockWise,
                Name = "Tomer"
            };
            var player2 = new Player
            {
                Color = "Black",
                Direction = DirectionEnum.AntiClockWise,
                Name = "Dror"
            };
            
            //Act
            var turn = game.StartGame(player1, player2);

            //Assert
        }
    }
}
