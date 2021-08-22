using EscapeMines.Common.Enums;

namespace EscapeMines.Common
{
    public class Turtle : GameItem
    {
        public TurtleDirection Direction { get; set; }

        public Turtle() : base()
        {
            ItemType = ItemType.Turtle;
        }

        public void CopyFrom(Turtle turtle)
        {
            Position = turtle.Position;
            Direction = turtle.Direction;
        }
    }
}
