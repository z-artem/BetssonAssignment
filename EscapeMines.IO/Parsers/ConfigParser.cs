using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.IO.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EscapeMines.IO.Parsers
{
    public class ConfigParser : IConfigParser
    {
        private readonly ILogger<ConfigParser> _logger;

        public ConfigParser(ILogger<ConfigParser> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public (int Width, int Height) ParseBoardDimensions(string rawConfig)
        {
            try
            {
                var dimensions = ParseCoordinates(rawConfig, ' ');
                return (dimensions.X, dimensions.Y);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ParseBoardDimensions failed due to exception: {ex.Message}");
                throw;
            }
        }

        public List<GameCommand> ParseCommands(string rawConfig)
        {
            try
            {
                var configParts = rawConfig.Split(' ');

                var commands = new List<GameCommand>();
                foreach (var rawCommand in configParts)
                {
                    commands.Add(rawCommand.ParseToEnum<GameCommand>());
                }

                return commands;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ParseCommands failed due to exception: {ex.Message}");
                throw;
            }
        }

        public GameItem ParseExit(string rawConfig)
        {
            try
            {
                return new GameItem(ItemType.Exit)
                {
                    Position = ParseCoordinates(rawConfig, ' ')
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ParseExit failed due to exception: {ex.Message}");
                throw;
            }
        }

        public List<GameItem> ParseMines(string rawConfig)
        {
            try
            {
                var configParts = rawConfig.Split(' ');

                var mines = new List<GameItem>();
                foreach (var rawMineLocation in configParts)
                {
                    mines.Add(new GameItem(ItemType.Mine)
                    {
                        Position = ParseCoordinates(rawMineLocation, ',')
                    });
                }

                return mines;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ParseMines failed due to exception: {ex.Message}");
                throw;
            }
        }

        public Turtle ParseTurtle(string rawConfig)
        {
            try
            {
                var configParts = rawConfig.Split(' ');
                return new Turtle
                {
                    Position = PositionFromStringParts(configParts),
                    Direction = configParts[2].ParseToEnum<TurtleDirection>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ParseTurtle failed due to exception: {ex.Message}");
                throw;
            }
        }

        private ItemPosition ParseCoordinates(string rawConfig, char delimiter)
        {
            var configParts = rawConfig.Split(delimiter);
            if (configParts.Length != 2)
                throw new ArgumentException($"Invalid coordinate string passed: '{rawConfig}'");

            return PositionFromStringParts(configParts);
        }

        private ItemPosition PositionFromStringParts(string[] parts) =>
            new ItemPosition
            {
                X = int.Parse(parts[0]),
                Y = int.Parse(parts[1])
            };
    }
}
