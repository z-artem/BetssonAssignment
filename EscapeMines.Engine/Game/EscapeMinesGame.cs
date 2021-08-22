using EscapeMines.Common;
using EscapeMines.Common.Enums;
using EscapeMines.Engine.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeMines.Engine.Game
{
    public class EscapeMinesGame : IEscapeMinesGame
    {
        private Turtle _turtle = new Turtle();
        private TurtleState _turtleState;

        private GameConfig _gameConfig;

        private IDictionary<GameCommand, Action> _commandHandlers;

        private readonly ILogger<EscapeMinesGame> _logger;
        private readonly IMoveHandler _moveHandler;
        private readonly ITurnHandler _turnHandler;

        public EscapeMinesGame(ILogger<EscapeMinesGame> logger, IMoveHandler moveHandler, ITurnHandler turnHandler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _moveHandler = moveHandler ?? throw new ArgumentNullException(nameof(moveHandler));
            _turnHandler = turnHandler ?? throw new ArgumentNullException(nameof(turnHandler));

            RegisterHandlers();
        }

        public TurtleState GetTurtleState() => _turtleState;

        public void SetConfig(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _moveHandler.SetBoardDimensions(_gameConfig.BoardDimensions.Width, _gameConfig.BoardDimensions.Height);

            Reset();
        }

        public void Reset()
        {
            _logger.LogInformation("Game reset");

            _turtle.CopyFrom(_gameConfig.Turtle);
            _turtleState = TurtleState.InDanger;
        }

        public void RunCommands() =>
            RunCommands(_gameConfig.Commands);

        public void RunCommands(IEnumerable<GameCommand> commands)
        {
            foreach (var command in commands)
            {
                RunCommand(command);
            }
        }

        public void RunCommand(GameCommand command)
        {
            if (GameOver())
                return;

            _commandHandlers[command]();
        }

        private void RegisterHandlers()
        {
            _commandHandlers = new Dictionary<GameCommand, Action>();

            _commandHandlers[GameCommand.Move] = Move;
            _commandHandlers[GameCommand.TurnLeft] = TurnLeft;
            _commandHandlers[GameCommand.TurnRight] = TurnRight;
        }

        private bool GameOver() =>
            _turtleState == TurtleState.Exploded || _turtleState == TurtleState.ExitReached;

        private void UpdateTurtleState()
        {
            var hitObject = _gameConfig.Items.FirstOrDefault(x => x.Position.EqualsTo(_turtle.Position));

            if (hitObject != null)
            {
                switch (hitObject.ItemType)
                {
                    case ItemType.Mine:
                        _turtleState = TurtleState.Exploded;
                        break;
                    case ItemType.Exit:
                        _turtleState = TurtleState.ExitReached;
                        break;
                }

                _logger.LogInformation($"Game is over with state: {_turtleState}");
            }
        }

        private void TurnLeft() =>
            _turtle.Direction = _turnHandler.TurnLeft(_turtle.Direction);

        private void TurnRight() =>
            _turtle.Direction = _turnHandler.TurnRight(_turtle.Direction);

        private void Move()
        {
            _turtle.Position = _moveHandler.Move(_turtle);

            UpdateTurtleState();
        }
    }
}
