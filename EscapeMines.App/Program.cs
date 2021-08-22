using EscapeMines.Engine;
using EscapeMines.Engine.Game;
using EscapeMines.IO;
using EscapeMines.IO.Extensions;
using Microsoft.Extensions.Configuration;
using System;

namespace EscapeMines.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                IGameConfigProvider gameConfigProvider = ConfigProviderBuilder.BuildConfigProvider();
                var gameConfig = gameConfigProvider.GetConfig(config["configPath"]);

                IEscapeMinesGame engine = GameBuilder.BuildGame();
                engine.SetConfig(gameConfig);
                engine.RunCommands();

                var turtleState = engine.GetTurtleState();
                turtleState.ToConsole();
            }
            catch (Exception)
            {
                Console.WriteLine($"Program execution terminated");
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
