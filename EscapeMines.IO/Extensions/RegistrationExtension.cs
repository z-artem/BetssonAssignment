using EscapeMines.IO.Parsers;
using EscapeMines.IO.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace EscapeMines.IO.Extensions
{
    public static class RegistrationExtension
    {
        public static void RegisterDependencies(this IServiceCollection dependencies)
        {
            dependencies.AddTransient<FileConfigProvider>();

            dependencies.AddTransient<IConfigParser, ConfigParser>();
            dependencies.AddTransient<IConfigValidator, ConfigValidator>();
        }
    }
}
