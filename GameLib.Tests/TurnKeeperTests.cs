using GameLib.Models;
using GameLib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameLib.Tests
{
    public class TurnKeeperTests
    {
        [Fact]
        public void IsTurnTracked()
        {
            //Arrange
            var turnKeeper = new TurnKeeper();

            var player1 = new Player
            {
                Direction = DirectionEnum.ClockWise,
                Name = "Tomer"
            };
            var player2 = new Player
            {
                Direction = DirectionEnum.AntiClockWise,
                Name = "Dror"
            };
            turnKeeper.AddPlayer(player1);
            turnKeeper.AddPlayer(player2);

            //Act 
            var activePlayer = turnKeeper.GetActivePlayer();

            //Assert
            Assert.Equal(player1, activePlayer);
        }
        [Fact]
        public void IsTurnChanges()
        {
            var turnKeeper = new TurnKeeper();

            var player1 = new Player
            {
                Direction = DirectionEnum.ClockWise,
                Name = "Tomer"
            };
            var player2 = new Player
            {
                Direction = DirectionEnum.AntiClockWise,
                Name = "Dror"
            };

            //act 
            turnKeeper.AddPlayer(player1);
            turnKeeper.AddPlayer(player2);
            turnKeeper.EndTurn(player1);
            var activePlayer = turnKeeper.GetActivePlayer();

            //Assert
            Assert.Equal(player2, activePlayer);
        }
    }
}
