using Microsoft.Extensions.Logging;
using System;

namespace FindMe.Web.App
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private Func<string, LogLevel, bool> _filter;

        public DatabaseLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(categoryName, _filter);
        }

        public void Dispose()
        {
            _filter = null;
        }
    }
}
