using FindMe.Data.Models;
using Microsoft.Extensions.Logging;
using Swapp.Data;
using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class DatabaseLogger : ILogger
    {
        private static bool _awaiting;


        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;

        private object _dbLogErrorLock;

        private Exception _lastDbLogException;

        public DatabaseLogger(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _categoryName = categoryName;
            _filter = filter;

            _dbLogErrorLock = new object();

            _lastDbLogException = null;
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel)
                && exception != null)
            {
                bool withStackTrace = (state != null && state.GetPropVal<bool>("withStackTrace"));

                _DbLogErrorASync += DatabaseLogger__DbLogErrorASync;
                _DbLogErrorASync(this, new ValueEventArgs<LogObj>(
                                        new LogObj()
                                        {
                                            Exception = exception,
                                            Description = eventId.Name,
                                            LogLevel = logLevel,
                                            WithStackTrace = withStackTrace,
                                            Repository = state.GetPropVal<WebDbRepository>("repository")
                                        }));
            }
        }

        private event EventHandler<ValueEventArgs<LogObj>> _DbLogErrorASync;
        private async void DatabaseLogger__DbLogErrorASync(object sender, ValueEventArgs<LogObj> e)
        {
            _DbLogErrorASync -= DatabaseLogger__DbLogErrorASync;

            Logging lgng = null;

            Func<Exception, object> func = null;

            try
            {
                if (e != null
                    && e.Value.Repository != null)
                {
                    func = (ex) =>
                    {
                        if (ex == null) return null;

                        return new
                        {
                            Message = ex.Message,
                            StackTrace = e.Value.WithStackTrace ? ex.StackTrace : "",
                            InnerException = func(ex.InnerException),
                            ID = (ex is ExceptionID) ? (int)((ExceptionID)ex).ErrorID : -1
                        };
                    };

                    lgng = new Logging()
                    {
                        Desc = e.Value.Description,
                        LogLevel = (short)e.Value.LogLevel,
                        Exception = Helper.JSonSerializeObject(func(e.Value.Exception))
                    };
                    await Task.Run(async () =>
                    {
                        while (_awaiting) await Task.Delay(200);
                    });

                    _awaiting = true;
                    await e.Value.Repository.Execute("AddLoggings", Helper.JSonSerializeObject(new object[] { lgng.Simplify() }));
                }
            }
            catch (Exception ex)
            {
                _lastDbLogException = ex;
            }
            finally
            {
                _awaiting = false;
                lgng = null;
                func = null;
            }
        }


        private struct LogObj
        {
            public WebDbRepository Repository { get; set; }
            public Exception Exception { get; set; }
            public string Description { get; set; }
            public LogLevel LogLevel { get; set; }
            public bool WithStackTrace { get; set; }
        }
    }
}
