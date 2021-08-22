using EscapeMines.Common.Enums;
using EscapeMines.Engine.Handlers;
using FluentAssertions;
using Xunit;

namespace EscapeMines.Engine.Tests.Handlers
{
    public class TurnHandlerTest
    {
        private readonly ITurnHandler _turnHandler;

        public TurnHandlerTest()
        {
            _turnHandler = new TurnHandler();
        }

        [Theory]
        [InlineData(TurtleDirection.North, TurtleDirection.East)]
        [InlineData(TurtleDirection.East, TurtleDirection.South)]
        [InlineData(TurtleDirection.South, TurtleDirection.West)]
        [InlineData(TurtleDirection.West, TurtleDirection.North)]
        public void TurnRight_TurnsRight(TurtleDirection beforeTurn, TurtleDirection expectedResult)
        {
            // act
            var actualResult = _turnHandler.TurnRight(beforeTurn);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(TurtleDirection.North, TurtleDirection.West)]
        [InlineData(TurtleDirection.East, TurtleDirection.North)]
        [InlineData(TurtleDirection.South, TurtleDirection.East)]
        [InlineData(TurtleDirection.West, TurtleDirection.South)]
        public void TurnLeft_TurnsLeft(TurtleDirection beforeTurn, TurtleDirection expectedResult)
        {
            // act
            var actualResult = _turnHandler.TurnLeft(beforeTurn);

            // assert
            actualResult.Should().Be(expectedResult);
        }
    }
}
