using EscapeMines.Common;
using FluentAssertions;
using Xunit;

namespace EscapeMines.Engine.Tests.Models
{
    public class ItemPositionTest
    {
        private readonly ItemPosition _itemPosition;

        public ItemPositionTest()
        {
            _itemPosition = new ItemPosition
            {
                X = 5,
                Y = 6
            };
        }

        [Theory]
        [InlineData(1, 1, false)]
        [InlineData(1, 6, false)]
        [InlineData(5, 1, false)]
        [InlineData(5, 6, true)]
        [InlineData(6, 5, false)]
        public void EqualsTo_ChecksIfEqual(int x, int y, bool expectedResult)
        {
            // arrange
            ItemPosition otherPosition = new ItemPosition
            {
                X = x,
                Y = y
            };

            // act
            var actualResult = _itemPosition.EqualsTo(otherPosition);

            // assert
            actualResult.Should().Be(expectedResult);
        }
    }
}
