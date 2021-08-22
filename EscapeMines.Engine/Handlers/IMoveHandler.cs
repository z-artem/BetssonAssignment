using EscapeMines.Common;

namespace EscapeMines.Engine.Handlers
{
    public interface IMoveHandler
    {
        void SetBoardDimensions(int width, int height);

        ItemPosition Move(Turtle turtle);
    }
}
