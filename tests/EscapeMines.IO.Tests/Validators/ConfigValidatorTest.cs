using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Tests;
using EscapeMines.Engine.Tests.Builders;
using EscapeMines.Engine.Tests.Helpers;
using EscapeMines.IO.Validators;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace EscapeMines.IO.Tests.Validators
{
    public class ConfigValidatorTest : TestBase<ConfigValidator>
    {
        public ConfigValidatorTest()
        {
            TestTarget = new ConfigValidator(_logger);
        }

        [Fact]
        public override void Constructor_ArgumentIsNull_Throws()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new ConfigValidator(null));
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 5, false)]
        [InlineData(5, 0, false)]
        [InlineData(1, 5, true)]
        [InlineData(5, 1, true)]
        [InlineData(1, 1, false)]
        [InlineData(3, 3, true)]
        public void ValidateBoardDimensions_Validates(int width, int height, bool expectedResult)
        {
            // arrange
            var config = GameConfigBuilder.Build();
            config.BoardDimensions = (width, height);

            TestTarget.SetConfig(config);

            // act
            var actualResult = TestTarget.ValidateBoardDimensions();

            // assert
            actualResult.Should().Be(expectedResult);
            if (expectedResult)
            {
                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
                _logger.VerifyLogged(LogLevel.Critical, $"ValidateBoardDimensions failed: incorrect board dimensions or board is unplayable - Width={width}, Height={height}");
            }
        }

        [Fact]
        public void ValidateGameObjects_ConfigOk_Passes()
        {
            // arrange
            var config = GameConfigBuilder.Build();

            TestTarget.SetConfig(config);

            // act
            var actualResult = TestTarget.ValidateGameObjects();

            // assert
            actualResult.Should().BeTrue();
            _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void ValidateGameObjects_DifferentExitsCount_Validates(int exitsCount, bool expectedResult)
        {
            // arrange
            var config = GameConfigBuilder.Build();
            var items = new List<GameItem>();
            for (int i = 0; i < exitsCount; i++)
            {
                items.Add(new GameItem(ItemType.Exit)
                {
                    Position = new ItemPosition
                    {
                        X = i,
                        Y = i
                    }
                });
            }

            config.Items = items;
            TestTarget.SetConfig(config);

            // act
            var actualResult = TestTarget.ValidateGameObjects();

            // assert
            actualResult.Should().Be(expectedResult);
            if (expectedResult)
            {
                _logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            }
            else
            {
                _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
                _logger.VerifyLogged(LogLevel.Critical, $"ValidateGameObjects failed: game config should have only one exit but now has {exitsCount}");
            }
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(10, 0)]
        [InlineData(0, 10)]
        [InlineData(10, 10)]
        public void ValidateGameObjects_ItemsOutOfBoard_Fails(int x, int y)
        {
            // arrange
            var config = GameConfigBuilder.Build();
            var items = new List<GameItem>();
            items.Add(new GameItem(ItemType.Exit)
            {
                Position = new ItemPosition
                {
                    X = x,
                    Y = y
                }
            });

            config.Items = items;
            config.BoardDimensions = (10, 10);
            TestTarget.SetConfig(config);

            // act
            var actualResult = TestTarget.ValidateGameObjects();

            // assert
            actualResult.Should().BeFalse();

            _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            _logger.VerifyLogged(LogLevel.Critical, $"ValidateGameObjects failed: game object is out of board - X={x}, Y={y}");
        }

        [Fact]
        public void ValidateGameObjects_MultipleItemsOnSamePosition_Fails()
        {
            // arrange
            var config = GameConfigBuilder.Build();
            var items = new List<GameItem>();
            items.Add(new GameItem(ItemType.Exit)
            {
                Position = new ItemPosition
                {
                    X = 2,
                    Y = 2
                }
            });
            for (int i = 0; i < 2; i++)
            {
                items.Add(new GameItem(ItemType.Mine)
                {
                    Position = new ItemPosition
                    {
                        X = 5,
                        Y = 5
                    }
                });
            }

            config.Items = items;
            config.BoardDimensions = (10, 10);
            TestTarget.SetConfig(config);

            // act
            var actualResult = TestTarget.ValidateGameObjects();

            // assert
            actualResult.Should().BeFalse();

            _logger.ReceivedWithAnyArgs(1).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), null, null, null);
            _logger.VerifyLogged(LogLevel.Critical, $"ValidateGameObjects failed: attempt to place more than one object to the same position - X={5}, Y={5}");
        }
    }
}
