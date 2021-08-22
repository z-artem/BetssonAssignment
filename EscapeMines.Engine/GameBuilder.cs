using EscapeMines.Engine.Game;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EscapeMines.Engine
{
    public static class GameBuilder
    {
        public static IEscapeMinesGame BuildGame()
        {
            using IHost host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((_, dependencies) => dependencies.RegisterDependencies())
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            return serviceScope.ServiceProvider.GetRequiredService<EscapeMinesGame>();
        }
    }
}
