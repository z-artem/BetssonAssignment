using EscapeMines.Common.Enums;
using System;

namespace EscapeMines.Common
{
    public class GameItem
    {
        public ItemPosition Position { get; set; }

        public ItemType ItemType { get; protected set; }

        public GameItem(ItemType itemType)
        {
            if (itemType == ItemType.Turtle) throw new InvalidOperationException("GameItem instantiation with type 'ItemType.Turtle' is not allowed. Use 'Turtle' class instead.");

            ItemType = itemType;
        }

        protected GameItem() { }
    }
}
