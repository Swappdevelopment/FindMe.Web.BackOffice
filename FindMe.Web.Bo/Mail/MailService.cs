using Mandrill;
using Mandrill.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
                _serverName = config["MandrillEmailManagement:host"];
                _userName = config["MandrillEmailManagement:smtpUserName"];
                _password = config["MandrillEmailManagement:apiKey"];

                _port = int.Parse(config["MandrillEmailManagement:port"]);
            }
        }

        public override async Task SendEmailAsync(
            string subject,
            string message,
            string toEmail = null,
            string fromEmail = null,
            string replyToEmail = null)
        {
            MandrillApi mandrillApi;
            MandrillMessage mandrillMessage;
            IList<MandrillSendMessageResponse> result = null;

            try
            {
                fromEmail = string.IsNullOrEmpty(fromEmail) ? _fromEmail : fromEmail;
                replyToEmail = string.IsNullOrEmpty(replyToEmail) ? fromEmail : replyToEmail;

                mandrillApi = new MandrillApi(_password);


                mandrillMessage = new MandrillMessage();
                mandrillMessage.Subject = subject;
                mandrillMessage.FromEmail = fromEmail;
                mandrillMessage.AddTo(toEmail);
                mandrillMessage.ReplyTo = replyToEmail;

                mandrillMessage.Text = message;

                result = await mandrillApi.Messages.SendAsync(mandrillMessage);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                mandrillApi = null;
                mandrillMessage = null;

                if (result != null)
                {
                    result.Clear();
                    result = null;
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
