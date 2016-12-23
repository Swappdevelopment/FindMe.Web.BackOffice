using Microsoft.AspNetCore.Hosting;
using Swapp.Data;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class DevMailService : BaseMailService
    {
        private IHostingEnvironment _env;

        public DevMailService(IHostingEnvironment env)
            : base()
        {
            _env = env;

            _baseEmailServerName = DevSecrets.GetSecretValue("swappAccount:serverName");
            _baseEmail = DevSecrets.GetSecretValue("swappAccount:email");
            _baseEmailPassword = DevSecrets.GetSecretValue("swappAccount:emailPassword");
        }


        public override async Task SendEmailAsync(string email, string subject, string message)
        {
            await base.SendEmailAsync(email, subject, message);
        }


        public override void Dispose()
        {
            base.Dispose();

            _env = null;
        }
    }
}
