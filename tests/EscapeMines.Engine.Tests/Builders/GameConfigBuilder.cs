using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.Engine.Tests.Builders
{
    public static class GameConfigBuilder
    {
        public static GameConfig Build()
        {
            return new GameConfig
            {
                BoardDimensions = (10, 11),
                Commands = BuildCommands(),
                Items = GameItemsBuilder.BuildMany(),
                Turtle = TurtleBuilder.Build(1, 1, TurtleDirection.East)
            };
        }

        public static IEnumerable<GameCommand> BuildCommands()
        {
            return new List<GameCommand>
            {
                GameCommand.Move,
                GameCommand.TurnRight,
                GameCommand.Move,
                GameCommand.TurnLeft,
                GameCommand.Move
            };
        }
    }
}
