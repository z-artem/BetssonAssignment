using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.Engine.Tests.Builders
{
    public static class GameItemsBuilder
    {
        public static GameItem Build(int x = 1, int y = 1, ItemType type = ItemType.Mine)
        {
            return new GameItem(type)
            {
                Position = new ItemPosition
                {
                    X = x,
                    Y = y
                }
            };
        }

        public static IEnumerable<GameItem> BuildMany()
        {
            return new List<GameItem>
            {
                Build(2, 2),
                Build(3, 3),
                Build(4, 4),
                Build(5, 5, ItemType.Exit)
            };
        }
    }
}
