using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.Common
{
    public class GameConfig
    {
        public (int Width, int Height) BoardDimensions { get; set; }

        public IEnumerable<GameItem> Items { get; set; }

        public Turtle Turtle { get; set; }

        public IEnumerable<GameCommand> Commands { get; set; }
    }
}
