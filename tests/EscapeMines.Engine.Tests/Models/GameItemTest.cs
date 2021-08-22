using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System;
using Xunit;

namespace EscapeMines.Engine.Tests.Models
{
    public class GameItemTest
    {
        [Fact]
        public void TryCreateTurtleAsItem_Throws()
        {
            // act & assert
            Assert.Throws<InvalidOperationException>(() => new GameItem(ItemType.Turtle));
        }
    }
}
