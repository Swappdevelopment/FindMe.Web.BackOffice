using System;

namespace FindMe.Web.App
{
    public class ConnectionStringManager : IDisposable
    {
        private ConnectionStringManager(Func<string, string> funcGetConnStr)
        {
            this.GetConnectionString = funcGetConnStr;
        }


        public Func<string, string> GetConnectionString { get; private set; }


        internal static ConnectionStringManager Create(Func<string, string> funcGetConnStr)
        {
            return new ConnectionStringManager(funcGetConnStr);
        }

        public void Dispose()
        {
            this.GetConnectionString = null;
        }
    }
}
