using FindMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class AppDbContext : PrjMsSqlDbContext
    {
        public AppDbContext(IConfigurationRoot config, DbContextOptions options)
            : base(config, options)
        {
        }
    }
}
