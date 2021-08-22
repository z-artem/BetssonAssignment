using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Handlers;
using EscapeMines.Engine.Tests.Builders;
using FluentAssertions;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace EscapeMines.Engine.Tests.Handlers
{
    public class MoveTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // free to move
            yield return new object[] { new ItemPosition { X = 5, Y = 5 }, TurtleDirection.North, new ItemPosition { X = 5, Y = 4 } };
            yield return new object[] { new ItemPosition { X = 5, Y = 5 }, TurtleDirection.East, new ItemPosition { X = 6, Y = 5 } };
            yield return new object[] { new ItemPosition { X = 5, Y = 5 }, TurtleDirection.South, new ItemPosition { X = 5, Y = 6 } };
            yield return new object[] { new ItemPosition { X = 5, Y = 5 }, TurtleDirection.West, new ItemPosition { X = 4, Y = 5 } };

            // wall ahead
            yield return new object[] { new ItemPosition { X = 5, Y = 0 }, TurtleDirection.North, new ItemPosition { X = 5, Y = 0 } };
            yield return new object[] { new ItemPosition { X = 9, Y = 5 }, TurtleDirection.East, new ItemPosition { X = 9, Y = 5 } };
            yield return new object[] { new ItemPosition { X = 5, Y = 9 }, TurtleDirection.South, new ItemPosition { X = 5, Y = 9 } };
            yield return new object[] { new ItemPosition { X = 0, Y = 5 }, TurtleDirection.West, new ItemPosition { X = 0, Y = 5 } };
        }
    }

    public class MoveHandlerTest
    {
        private readonly IMoveHandler _handler;

        public MoveHandlerTest()
        {
            _handler = new MoveHandler();
            _handler.SetBoardDimensions(10, 10);
        }

        [Theory]
        [MoveTestData]
        public void Move_HandlesMove(ItemPosition turtlePosition, TurtleDirection turtleDirection, ItemPosition expectedResult)
        {
            // arrange
            var turtle = TurtleBuilder.Build(turtlePosition.X, turtlePosition.Y, turtleDirection);

            // act
            var actualResult = _handler.Move(turtle);

            // assert
            actualResult.EqualsTo(expectedResult).Should().BeTrue();
        }
    }
}
