using Microsoft.Extensions.Logging;
using NSubstitute;

namespace EscapeMines.Engine.Tests
{
    public abstract class TestBase<T> where T : class
    {
        protected T TestTarget { get; set; }

        protected readonly ILogger<T> _logger;

        protected TestBase()
        {
            _logger = Substitute.For<ILogger<T>>();
        }

        public abstract void Constructor_ArgumentIsNull_Throws();
    }
}
