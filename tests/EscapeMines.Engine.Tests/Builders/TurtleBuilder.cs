using EscapeMines.Common;
using EscapeMines.Common.Enums;

namespace EscapeMines.Engine.Tests.Builders
{
    public static class TurtleBuilder
    {
        public static Turtle Build(int x = 0, int y = 0, TurtleDirection direction = TurtleDirection.North)
        {
            return new Turtle
            {
                Direction = direction,
                Position = new ItemPosition
                {
                    X = x,
                    Y = y
                }
            };
        }
    }
}
