using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Game;
using EscapeMines.Engine.Handlers;
using EscapeMines.Engine.Tests.Builders;
using EscapeMines.Engine.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EscapeMines.Engine.Tests.Game
{
    public class EscapeMinesGameTest : TestBase<EscapeMinesGame>
    {
        private readonly IMoveHandler _moveHandler;
        private readonly ITurnHandler _turnHandler;

        public EscapeMinesGameTest()
        {
            _moveHandler = Substitute.For<IMoveHandler>();
            _turnHandler = Substitute.For<ITurnHandler>();

            TestTarget = new EscapeMinesGame(_logger, _moveHandler, _turnHandler);
        }

        [Fact]
        public override void Constructor_ArgumentIsNull_Throws()
        {
            // act & assert
            AssertHelper.ThrowsArgumentNullException(Tuple.Create(_logger, _moveHandler, _turnHandler),
                t => new EscapeMinesGame(t.Item1, t.Item2, t.Item3));
        }

        [Fact]
        public void SetConfig_ArgumentIsOk_SetsConfig()
        {
            // arrange
            var config = GameConfigBuilder.Build();

            // act
            TestTarget.SetConfig(config);

            // assert
            _moveHandler.Received(1).SetBoardDimensions(config.BoardDimensions.Width, config.BoardDimensions.Height);

            _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            _logger.VerifyLogged(LogLevel.Information, "Game reset");
        }

        [Fact]
        public void RunCommands_NoArgs_RunsFromConfig()
        {
            // arrange
            var config = GameConfigBuilder.Build();
            config.Commands = new List<GameCommand>
            {
                GameCommand.Move,
                GameCommand.Move
            };

            TestTarget.SetConfig(config);

            _moveHandler.Move(null).ReturnsForAnyArgs(config.Turtle.Position);

            // act
            TestTarget.RunCommands();

            // assert
            _moveHandler.ReceivedWithAnyArgs(config.Commands.Count()).Move(null);
        }

        [Fact]
        public void RunCommands_WithArgs_RunsFromArgs()
        {
            // arrange
            var config = GameConfigBuilder.Build();
            config.Commands = new List<GameCommand>
            {
                GameCommand.Move,
                GameCommand.Move
            };

            var externalCommands = new List<GameCommand>
            {
                GameCommand.TurnRight,
                GameCommand.TurnRight,
                GameCommand.TurnRight
            };

            TestTarget.SetConfig(config);

            _turnHandler.TurnRight(config.Turtle.Direction).ReturnsForAnyArgs(config.Turtle.Direction);

            // act
            TestTarget.RunCommands(externalCommands);

            // assert
            _moveHandler.DidNotReceiveWithAnyArgs().Move(null);
            _turnHandler.ReceivedWithAnyArgs(externalCommands.Count()).TurnRight(config.Turtle.Direction);
        }

        [Theory]
        [InlineData(GameCommand.TurnLeft)]
        [InlineData(GameCommand.TurnRight)]
        [InlineData(GameCommand.Move)]
        public void RunCommand_ArgumentIsOk_CallsProperHadler(GameCommand command)
        {
            // arrange
            var config = GameConfigBuilder.Build();

            TestTarget.SetConfig(config);

            _turnHandler.TurnLeft(config.Turtle.Direction).ReturnsForAnyArgs(config.Turtle.Direction);
            _turnHandler.TurnRight(config.Turtle.Direction).ReturnsForAnyArgs(config.Turtle.Direction);
            _moveHandler.Move(null).ReturnsForAnyArgs(config.Turtle.Position);

            // act
            TestTarget.RunCommand(command);

            // assert
            _turnHandler.ReceivedWithAnyArgs(command == GameCommand.TurnLeft ? 1 : 0).TurnLeft(config.Turtle.Direction);
            _turnHandler.ReceivedWithAnyArgs(command == GameCommand.TurnRight ? 1 : 0).TurnRight(config.Turtle.Direction);
            _moveHandler.ReceivedWithAnyArgs(command == GameCommand.Move ? 1 : 0).Move(null);
        }

        [Theory]
        [InlineData(null, TurtleState.InDanger)]
        [InlineData(ItemType.Mine, TurtleState.Exploded)]
        [InlineData(ItemType.Exit, TurtleState.ExitReached)]
        public void TurtleState_HitsObject_ReturnsState(ItemType? hitObjectType, TurtleState expectedResult)
        {
            // arrange
            var objectPosition = new ItemPosition()
            {
                X = 1,
                Y = 0
            };

            var items = new List<GameItem>();
            if (hitObjectType.HasValue)
            {
                items.Add(new GameItem(hitObjectType.Value)
                {
                    Position = objectPosition
                });
            }

            var config = GameConfigBuilder.Build();
            config.Turtle = TurtleBuilder.Build(1, 1, TurtleDirection.North);
            config.Items = items;

            TestTarget.SetConfig(config);

            _moveHandler.Move(null).ReturnsForAnyArgs(objectPosition);

            // act
            TestTarget.RunCommand(GameCommand.Move);
            var actualResult = TestTarget.GetTurtleState();

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void Reset_ResetsState()
        {
            // arrange
            var objectPosition = new ItemPosition()
            {
                X = 1,
                Y = 0
            };

            var config = GameConfigBuilder.Build();
            config.Turtle = TurtleBuilder.Build(1, 1, TurtleDirection.North);
            config.Items = new List<GameItem>
            {
                new GameItem(ItemType.Mine)
                {
                    Position = objectPosition
                }
            };

            TestTarget.SetConfig(config);

            _moveHandler.Move(null).ReturnsForAnyArgs(objectPosition);

            TestTarget.RunCommand(GameCommand.Move);
            TestTarget.GetTurtleState().Should().Be(TurtleState.Exploded);

            // act
            TestTarget.Reset();
            var actualResult = TestTarget.GetTurtleState();

            // assert
            actualResult.Should().Be(TurtleState.InDanger);
        }
    }
}
