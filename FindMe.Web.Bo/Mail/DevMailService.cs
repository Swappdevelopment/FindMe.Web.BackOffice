using Swapp.Data;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class DevMailService : BaseMailService
    {
        public DevMailService()
            : base()
        {
            _serverName = DevSecrets.GetSecretValue("swappAccount:serverName");
            _userName = DevSecrets.GetSecretValue("swappAccount:email");
            _fromEmail = DevSecrets.GetSecretValue("swappAccount:email");
            _password = DevSecrets.GetSecretValue("swappAccount:emailPassword");
        }


        public override async Task SendEmailAsync(
            string subject,
            string message,
            string toEmail = null,
            string fromEmail = null,
            string replyToEmail = null)
        {
            await base.SendEmailAsync(subject, message, toEmail: toEmail, fromEmail: fromEmail, replyToEmail: replyToEmail);
        }
    }
}
