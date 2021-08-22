using EscapeMines.Common.Enums;

namespace EscapeMines.Engine.Handlers
{
    public interface ITurnHandler
    {
        TurtleDirection TurnLeft(TurtleDirection turtleDirection);

        TurtleDirection TurnRight(TurtleDirection turtleDirection);
    }
}
