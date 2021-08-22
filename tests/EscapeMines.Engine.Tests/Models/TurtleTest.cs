using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Tests.Builders;
using FluentAssertions;
using Xunit;

namespace EscapeMines.Engine.Tests.Models
{
    public class TurtleTest
    {
        private readonly Turtle _turtle;

        public TurtleTest()
        {
            _turtle = TurtleBuilder.Build(0, 0, TurtleDirection.North);
        }

        [Fact]
        public void CopyFrom_ArgumentIsOk_Copies()
        {
            // arrange
            var source = TurtleBuilder.Build(5, 5, TurtleDirection.South);

            // act
            _turtle.CopyFrom(source);

            // assert
            _turtle.Position.EqualsTo(source.Position).Should().BeTrue();
            _turtle.Direction.Should().Be(source.Direction);
        }
    }
}
