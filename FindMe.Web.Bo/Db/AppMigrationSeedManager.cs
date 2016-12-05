using FindMe.Data;

namespace FindMe.Web.App
{
    public class AppMigrationSeedManager : MigrationSeedManager
    {
        public AppMigrationSeedManager(AppDbContext context)
            : base(context)
        {
        }
    }
}
