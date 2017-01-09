using FindMe.Data;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Web.App
{
    public class AppDbContext : PrjMySqlDbContext
    {
        public AppDbContext(ConnectionStringManager connStrMngr, DbContextOptions options)
            : base(connStrMngr == null ? null : connStrMngr.GetConnectionString, options)
        {
        }
    }
}
