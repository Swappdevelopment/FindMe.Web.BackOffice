using FindMe.Data;

namespace FindMe.Web.App
{
    public class AppDbInteractor : DbInteractor
    {
        public AppDbInteractor(AppDbContext context)
            : base(context)
        {
        }
    }
}
