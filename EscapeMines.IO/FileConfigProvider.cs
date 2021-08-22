using EscapeMines.Common;
using EscapeMines.IO.Extensions;
using EscapeMines.IO.Parsers;
using EscapeMines.IO.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace EscapeMines.IO
{
    public class FileConfigProvider : IGameConfigProvider
    {
        private readonly ILogger<FileConfigProvider> _logger;
        private readonly IConfigParser _configParser;
        private readonly IConfigValidator _configValidator;

        public FileConfigProvider(ILogger<FileConfigProvider> logger, IConfigParser configParser, IConfigValidator configValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configParser = configParser ?? throw new ArgumentNullException(nameof(configParser));
            _configValidator = configValidator ?? throw new ArgumentNullException(nameof(configValidator));
        }

        public GameConfig GetConfig(string source)
        {
            Dictionary<ConfigLineType, string> rawConfig = ReadRawConfig(source);
            if (rawConfig == null)
                return null;

            var gameItems = _configParser.ParseMines(rawConfig[ConfigLineType.Mines]);
            var exit = _configParser.ParseExit(rawConfig[ConfigLineType.Exit]);
            gameItems.Add(exit);

            var config = new GameConfig
            {
                BoardDimensions = _configParser.ParseBoardDimensions(rawConfig[ConfigLineType.BoardDimensions]),
                Items = gameItems,
                Turtle = _configParser.ParseTurtle(rawConfig[ConfigLineType.Turtle]),
                Commands = _configParser.ParseCommands(rawConfig[ConfigLineType.Commands]),
            };

            if (!ValidateConfig(config))
                return null;

            return config;
        }

        private Dictionary<ConfigLineType, string> ReadRawConfig(string filePath)
        {
            StreamReader configStream = new StreamReader(filePath);

            Dictionary<ConfigLineType, string> rawConfig = new Dictionary<ConfigLineType, string>();

            try
            {
                rawConfig[ConfigLineType.BoardDimensions] = configStream.ReadLine().NormalizeConfigLine();
                rawConfig[ConfigLineType.Mines] = configStream.ReadLine().NormalizeConfigLine();
                rawConfig[ConfigLineType.Exit] = configStream.ReadLine().NormalizeConfigLine();
                rawConfig[ConfigLineType.Turtle] = configStream.ReadLine().NormalizeConfigLine();
                rawConfig[ConfigLineType.Commands] = configStream.ReadToEnd().NormalizeConfigLine(true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to read game config file due to exception: {ex.Message}");
                return null;
            }

            return rawConfig;
        }

        private bool ValidateConfig(GameConfig config)
        {
            _configValidator.SetConfig(config);

            return _configValidator.ValidateBoardDimensions() &&
                _configValidator.ValidateGameObjects();
        }
    }
}
