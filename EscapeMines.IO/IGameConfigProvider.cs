using EscapeMines.Common;

namespace EscapeMines.IO
{
    public interface IGameConfigProvider
    {
        GameConfig GetConfig(string source);
    }
}
