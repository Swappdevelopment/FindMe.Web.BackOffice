using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class DevMailService : BaseMailService
    {
        private IHostingEnvironment _env;

        private string _rootPath;

        public DevMailService(IHostingEnvironment env)
            : base()
        {
            _env = env;

            _rootPath = _env == null ? null : _env.WebRootPath;

            if (string.IsNullOrEmpty(_rootPath))
            {
                _baseEmail = null;
                _baseEmailPassword = null;
            }
            else
            {
                _baseEmailServerName = DevSecrets.GetSecretValue("swappAccount:serverName", _rootPath);
                _baseEmail = DevSecrets.GetSecretValue("swappAccount:email", _rootPath);
                _baseEmailPassword = DevSecrets.GetSecretValue("swappAccount:emailPassword", _rootPath);
            }
        }


        public override async Task SendEmailAsync(string email, string subject, string message)
        {
            await base.SendEmailAsync(email, subject, message);
        }


        public override void Dispose()
        {
            base.Dispose();

            _env = null;
            _rootPath = null;
        }
    }
}
