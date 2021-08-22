using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.Engine.Game
{
    public interface IEscapeMinesGame
    {
        void SetConfig(GameConfig gameConfig);

        void RunCommand(GameCommand command);

        void RunCommands();

        void RunCommands(IEnumerable<GameCommand> commands);

        TurtleState GetTurtleState();

        void Reset();
    }
}
