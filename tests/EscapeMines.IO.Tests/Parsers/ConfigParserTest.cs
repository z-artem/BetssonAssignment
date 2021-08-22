using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Tests;
using EscapeMines.IO.Parsers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace EscapeMines.IO.Tests.Parsers
{
    public class ConfigParserTest : TestBase<ConfigParser>
    {
        public ConfigParserTest()
        {
            TestTarget = new ConfigParser(_logger);
        }

        [Fact]
        public override void Constructor_ArgumentIsNull_Throws()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new ConfigParser(null));
        }

        [Theory]
        [InlineData("1 2", true, 1, 2)]
        [InlineData("-12 22", true, -12, 22)]
        [InlineData("4 -13", true, 4, -13)]
        [InlineData("12", false, 0, 0)]
        [InlineData("1 e2", false, 0, 0)]
        [InlineData("what?", false, 0, 0)]
        [InlineData("", false, 0, 0)]
        [InlineData("3", false, 0, 0)]
        public void ParseBoardDimensions_Parses(string raw, bool expectedResult, int width, int height)
        {
            // act
            (int Width, int Height) actualResult = (0, 0);

            if (expectedResult)
                actualResult = TestTarget.ParseBoardDimensions(raw);
            else
                Assert.ThrowsAny<Exception>(() => TestTarget.ParseBoardDimensions(raw));

            // assert
            if (expectedResult)
            {
                actualResult.Width.Should().Be(width);
                actualResult.Height.Should().Be(height);

                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
        }

        [Theory]
        [MinesTestData]
        public void ParseMines_Parses(string raw, bool expectedResult, List<GameItem> mines)
        {
            // act
            List<GameItem> actualResult = null;

            if (expectedResult)
                actualResult = TestTarget.ParseMines(raw);
            else
                Assert.ThrowsAny<Exception>(() => TestTarget.ParseMines(raw));

            // assert
            if (expectedResult)
            {
                actualResult.Count.Should().Be(mines.Count);
                for (int i = 0; i < actualResult.Count; i++)
                {
                    actualResult[i].ItemType.Should().Be(ItemType.Mine);
                    actualResult[i].Position.EqualsTo(mines[i].Position).Should().BeTrue();
                }

                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
        }

        [Theory]
        [ExitTestData]
        public void ParseExit_Parses(string raw, bool expectedResult, GameItem exit)
        {
            // act
            GameItem actualResult = null;

            if (expectedResult)
                actualResult = TestTarget.ParseExit(raw);
            else
                Assert.ThrowsAny<Exception>(() => TestTarget.ParseExit(raw));

            // assert
            if (expectedResult)
            {
                actualResult.ItemType.Should().Be(ItemType.Exit);
                actualResult.Position.EqualsTo(exit.Position).Should().BeTrue();

                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
        }

        [Theory]
        [TurtleTestData]
        public void ParseTurtle_Parses(string raw, bool expectedResult, Turtle turtle)
        {
            // act
            Turtle actualResult = null;

            if (expectedResult)
                actualResult = TestTarget.ParseTurtle(raw);
            else
                Assert.ThrowsAny<Exception>(() => TestTarget.ParseTurtle(raw));

            // assert
            if (expectedResult)
            {
                actualResult.Direction.Should().Be(turtle.Direction);
                actualResult.Position.EqualsTo(turtle.Position).Should().BeTrue();

                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
        }

        [Theory]
        [CommandsTestData]
        public void ParseCommands_Parses(string raw, bool expectedResult, List<GameCommand> commands)
        {
            // act
            List<GameCommand> actualResult = null;

            if (expectedResult)
                actualResult = TestTarget.ParseCommands(raw);
            else
                Assert.ThrowsAny<Exception>(() => TestTarget.ParseCommands(raw));

            // assert
            if (expectedResult)
            {
                actualResult.Count.Should().Be(commands.Count);
                for (int i = 0; i < actualResult.Count; i++)
                {
                    actualResult[i].Should().Be(commands[i]);
                }

                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
        }
    }
}
