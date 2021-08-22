using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.Engine.Handlers
{
    public class TurnHandler : ITurnHandler
    {
        public static List<TurtleDirection> TurnList = new List<TurtleDirection>
        {
            TurtleDirection.North,
            TurtleDirection.East,
            TurtleDirection.South,
            TurtleDirection.West
        };

        public TurtleDirection TurnLeft(TurtleDirection turtleDirection)
        {
            var index = TurnList.IndexOf(turtleDirection);
            index--;
            if (index < 0)
                index = TurnList.Count - 1;

            return TurnList[index];
        }

        public TurtleDirection TurnRight(TurtleDirection turtleDirection)
        {
            var index = TurnList.IndexOf(turtleDirection);
            index++;
            if (index >= TurnList.Count)
                index = 0;

            return TurnList[index];
        }
    }
}
