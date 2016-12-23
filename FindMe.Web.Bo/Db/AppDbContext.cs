using FindMe.Data;
using Microsoft.EntityFrameworkCore;
using Swapp.Data;

namespace FindMe.Web.App
{
    public class AppDbContext : PrjMsSqlDbContext
    {
        public AppDbContext(ConnectionStringManager connStrMngr, DbContextOptions options)
            : base(connStrMngr == null ? null : connStrMngr.GetConnectionString, options)
        {
        }
    }
}
