namespace EscapeMines.Common
{
    public class ItemPosition
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool EqualsTo(ItemPosition itemPosition) =>
            X == itemPosition.X && Y == itemPosition.Y;
    }
}
