using EscapeMines.Common;
using EscapeMines.Common.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeMines.IO.Validators
{
    public class ConfigValidator : IConfigValidator
    {
        private readonly ILogger<ConfigValidator> _logger;

        private GameConfig _gameConfig;

        public ConfigValidator(ILogger<ConfigValidator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetConfig(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public bool ValidateBoardDimensions()
        {
            if (_gameConfig.BoardDimensions.Width < 1 ||
                _gameConfig.BoardDimensions.Height < 1 ||
                _gameConfig.BoardDimensions.Width == 1 && _gameConfig.BoardDimensions.Height == 1)
            {
                _logger.LogCritical($"ValidateBoardDimensions failed: incorrect board dimensions or board is unplayable - Width={_gameConfig.BoardDimensions.Width}, Height={_gameConfig.BoardDimensions.Height}");
                return false;
            }

            return true;
        }

        public bool ValidateGameObjects()
        {
            int exitsCount = _gameConfig.Items.Count(x => x.ItemType == ItemType.Exit);
            if (exitsCount != 1)
            {
                _logger.LogCritical($"ValidateGameObjects failed: game config should have only one exit but now has {exitsCount}");
                return false;
            }

            var allObjects = new List<(int X, int Y)>(_gameConfig.Items.Select(x => (x.Position.X, x.Position.Y)));
            allObjects.Add((_gameConfig.Turtle.Position.X, _gameConfig.Turtle.Position.Y));

            for (int i = 0; i < allObjects.Count - 1; i++)
            {
                var obj1 = allObjects[i];

                if (obj1.X < 0 || obj1.X >= _gameConfig.BoardDimensions.Width ||
                    obj1.Y < 0 || obj1.Y >= _gameConfig.BoardDimensions.Height)
                {
                    _logger.LogCritical($"ValidateGameObjects failed: game object is out of board - X={obj1.X}, Y={obj1.Y}");
                    return false;
                }

                for (int j = i + 1; j < allObjects.Count; j++)
                {
                    var obj2 = allObjects[j];

                    if (obj1.X == obj2.X && obj1.Y == obj2.Y)
                    {
                        _logger.LogCritical($"ValidateGameObjects failed: attempt to place more than one object to the same position - X={obj1.X}, Y={obj1.Y}");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
