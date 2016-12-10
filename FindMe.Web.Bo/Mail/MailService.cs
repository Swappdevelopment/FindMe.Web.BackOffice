using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class MailService : BaseMailService
    {
        public MailService()
            : base()
        {
        }


        public override async Task SendEmailAsync(string email, string subject, string message)
        {
            await base.SendEmailAsync(email, subject, message);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
