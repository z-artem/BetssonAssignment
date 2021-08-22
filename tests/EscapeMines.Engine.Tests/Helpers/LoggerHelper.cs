using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;

namespace EscapeMines.Engine.Tests.Helpers
{
    public static class LoggerHelper
    {
        public static void VerifyLogged(this ILogger logger, LogLevel logLevel, string logMessage = null)
        {
            var args = logger.ReceivedCalls().ElementAt(0).GetArguments();

            ((LogLevel)args[0]).Should().Be(logLevel);
            if (!string.IsNullOrEmpty(logMessage))
                (args[2].As<IReadOnlyList<KeyValuePair<string, object>>>()[0].Value as string).Should().Be(logMessage);
        }
    }
}
