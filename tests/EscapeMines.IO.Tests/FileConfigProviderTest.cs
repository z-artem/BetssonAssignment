using EscapeMines.Common.Enums;
using EscapeMines.Engine.Tests;
using EscapeMines.Engine.Tests.Builders;
using EscapeMines.Engine.Tests.Helpers;
using EscapeMines.IO.Parsers;
using EscapeMines.IO.Validators;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace EscapeMines.IO.Tests
{
    public class FileConfigProviderTest : TestBase<FileConfigProvider>
    {
        private readonly IConfigParser _configParser;
        private readonly IConfigValidator _configValidator;

        public FileConfigProviderTest()
        {
            _configParser = Substitute.For<IConfigParser>();
            _configValidator = Substitute.For<IConfigValidator>();

            TestTarget = new FileConfigProvider(_logger, _configParser, _configValidator);
        }

        [Fact]
        public override void Constructor_ArgumentIsNull_Throws()
        {
            // act & assert
            AssertHelper.ThrowsArgumentNullException(Tuple.Create(_logger, _configParser, _configValidator),
                t => new FileConfigProvider(t.Item1, t.Item2, t.Item3));
        }

        [Fact]
        public void GetConfig_FileIsOk_GetsConfig()
        {
            // arrange
            var expectedResult = GameConfigBuilder.Build();

            _configParser.ParseBoardDimensions(Arg.Any<string>()).ReturnsForAnyArgs(expectedResult.BoardDimensions);
            _configParser.ParseMines(Arg.Any<string>()).ReturnsForAnyArgs(expectedResult.Items.Where(x => x.ItemType == ItemType.Mine).ToList());
            _configParser.ParseExit(Arg.Any<string>()).ReturnsForAnyArgs(expectedResult.Items.Single(x => x.ItemType == ItemType.Exit));
            _configParser.ParseTurtle(Arg.Any<string>()).ReturnsForAnyArgs(expectedResult.Turtle);
            _configParser.ParseCommands(Arg.Any<string>()).ReturnsForAnyArgs(expectedResult.Commands);

            _configValidator.ValidateBoardDimensions().Returns(true);
            _configValidator.ValidateGameObjects().Returns(true);

            // act
            var actualResult = TestTarget.GetConfig("./TestData/ConfigOk.txt");

            // assert
            actualResult.Should().NotBeNull();

            _configValidator.ReceivedWithAnyArgs(1).SetConfig(null);
            _configValidator.ReceivedWithAnyArgs(1).ValidateBoardDimensions();
            _configValidator.ReceivedWithAnyArgs(1).ValidateGameObjects();
        }

        [Fact]
        public void GetConfig_FileEmpty_ReturnsNull()
        {
            // act
            var actualResult = TestTarget.GetConfig($"./TestData/ConfigEmpty.txt");

            //assert
            actualResult.Should().BeNull();
            _logger.VerifyLogged(LogLevel.Critical);
        }
    }
}
