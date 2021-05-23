using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GameLib.Tests
{
    public class GameBoardServiceTests
    {
        [Fact]
        public void IsBoardCorrectLength()
        {
            //Arrange
            var factory = new Services.GameBoardFactory(new Services.GamePieceFactory());
            //Act
            var board = factory.Create();
            //Assert
            var expected = 24; //1 - 24,
            Assert.Equal(expected, board.Triangles.Count);
        }
        [Fact]
        public void IsBarEmpty()
        {
            //Arrange
            var factory = new Services.GameBoardFactory(new Services.GamePieceFactory());
            //Act
            var board = factory.Create();
            //Assert
            Assert.True(board.Bar.IsEmpty);
        }
        [Fact]
        public void IsSafePiecesEmpty()
        {
            //Arrange
            var factory = new Services.GameBoardFactory(new Services.GamePieceFactory());
            //Act
            var board = factory.Create();
            //Assert
            var expected = 0;
            Assert.Equal(expected, board.SafePieces.Count);
        }
        [Fact]
        public void IsPiecesInCorrectPlace()
        {
            //Arrange
            var factory = new Services.GameBoardFactory(new Services.GamePieceFactory());
            int cnt = 1;
            //Act
            var board = factory.Create().Triangles.Values.OrderBy(t=>t.Position);
            //Assert
            foreach (var triangle in board)
            {
                
                (int position, string color, int amount) expected = (cnt++, "", 0); //1 - 24
                if (triangle.Position == 1)
                {
                    expected = (1, "Red", 2);
                }
                else if (triangle.Position == 6)
                {
                    expected = (6, "Black", 5);
                }
                else if (triangle.Position == 8)
                {
                    expected = (8, "Black", 3);
                }
                else if (triangle.Position == 12)
                {
                    expected = (12, "Red", 5);
                }
                else if (triangle.Position == 13)
                {
                    expected = (13, "Black", 5);
                }
                else if (triangle.Position == 17)
                {
                    expected = (17, "Red", 3);
                }
                else if (triangle.Position == 19)
                {
                    expected = (19, "Red", 5);
                }
                else if (triangle.Position == 24)
                {
                    expected = (24, "Black", 2);
                }
                Assert.Equal(expected.color, triangle.PiecesColor);
                Assert.Equal(expected.position, triangle.Position);
                Assert.Equal(expected.amount, triangle.GamePieces.Count);
            }
        }
    }
}
