using EscapeMines.Engine.Game;
using EscapeMines.Engine.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace EscapeMines.Engine
{
    public static class RegistrationExtension
    {
        public static void RegisterDependencies(this IServiceCollection dependencies)
        {
            dependencies.AddTransient<EscapeMinesGame>();

            dependencies.AddTransient<IMoveHandler, MoveHandler>();
            dependencies.AddTransient<ITurnHandler, TurnHandler>();
        }
    }
}
