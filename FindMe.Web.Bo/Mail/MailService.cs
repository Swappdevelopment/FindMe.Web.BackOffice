using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class MailService : BaseMailService
    {
        public MailService(IConfigurationRoot config)
            : base()
        {
            if (config != null)
            {
                _baseEmailServerName = config["ManagerEmailAccount:ServerName"];
                _baseEmail = config["ManagerEmailAccount:Email"];
                _baseEmailPassword = config["ManagerEmailAccount:EmailPassword"];
            }
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
