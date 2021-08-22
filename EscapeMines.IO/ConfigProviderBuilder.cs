using EscapeMines.IO.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EscapeMines.IO
{
    public static class ConfigProviderBuilder
    {
        public static IGameConfigProvider BuildConfigProvider()
        {
            using IHost host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((_, dependencies) => dependencies.RegisterDependencies())
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            return serviceScope.ServiceProvider.GetRequiredService<FileConfigProvider>();
        }
    }
}
