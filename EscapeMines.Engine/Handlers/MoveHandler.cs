using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System;
using System.Collections.Generic;

namespace EscapeMines.Engine.Handlers
{
    public class MoveHandler : IMoveHandler
    {
        private int _width;
        private int _height;

        private IDictionary<TurtleDirection, Func<ItemPosition, ItemPosition>> _moveHandlers;

        public MoveHandler()
        {
            RegisterHandlers();
        }

        public void SetBoardDimensions(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public ItemPosition Move(Turtle turtle)
        {
            return _moveHandlers[turtle.Direction](turtle.Position);
        }

        private void RegisterHandlers()
        {
            _moveHandlers = new Dictionary<TurtleDirection, Func<ItemPosition, ItemPosition>>();

            _moveHandlers[TurtleDirection.North] = MoveNorth;
            _moveHandlers[TurtleDirection.West] = MoveWest;
            _moveHandlers[TurtleDirection.South] = MoveSouth;
            _moveHandlers[TurtleDirection.East] = MoveEast;
        }

        private ItemPosition MoveNorth(ItemPosition position) => new ItemPosition
        {
            X = position.X,
            Y = position.Y == 0 ? 0 : position.Y - 1
        };

        private ItemPosition MoveSouth(ItemPosition position) => new ItemPosition
        {
            X = position.X,
            Y = position.Y == _height - 1 ? _height - 1 : position.Y + 1
        };

        private ItemPosition MoveWest(ItemPosition position) => new ItemPosition
        {
            X = position.X == 0 ? 0 : position.X - 1,
            Y = position.Y
        };

        private ItemPosition MoveEast(ItemPosition position) => new ItemPosition
        {
            X = position.X == _width - 1 ? _width - 1 : position.X + 1,
            Y = position.Y
        };
    }
}
