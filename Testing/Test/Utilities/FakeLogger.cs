using System;
using Microsoft.Extensions.Logging;

namespace Test.Utilities
{
    public class FakeLogger<T> : ILogger<T>, IDisposable
    {
        public LogLevel AddedLogLevel;
        public string LoggedMessage;
        public Exception LoggedException;
        public bool IsLogAdded;
        public static FakeLogger<T> Create() => new();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            AddedLogLevel = logLevel;
            LoggedMessage = state.ToString();
            LoggedException = exception;
            IsLogAdded = true;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => this;

        public void Dispose()
        {
        }
    }
}