using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System.Collections.Generic;

namespace EscapeMines.IO.Parsers
{
    public interface IConfigParser
    {
        (int Width, int Height) ParseBoardDimensions(string rawConfig);

        List<GameItem> ParseMines(string rawConfig);

        GameItem ParseExit(string rawConfig);

        Turtle ParseTurtle(string rawConfig);

        List<GameCommand> ParseCommands(string rawConfig);
    }
}
