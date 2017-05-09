using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class BaseMailService : IMailService
    {
        private const int DEFAULT_PORT = 1025;


        protected string _serverName;
        protected string _fromEmail;
        protected string _userName;
        protected string _password;

        protected string _host;
        protected int _port;
        protected bool _useSSL;

        public BaseMailService()
        {
            _serverName = null;
            _fromEmail = null;
            _userName = null;
            _password = null;

            _host = null;
            _port = DEFAULT_PORT;

            _useSSL = false;
        }

        protected void ValidateValues()
        {
            _userName = string.IsNullOrEmpty(_userName) ? _fromEmail : _userName;

            _port = _port > 0 ? _port : DEFAULT_PORT;
        }


        public virtual async Task SendEmailAsync(
            string subject,
            string message,
            string toEmail = null,
            string fromEmail = null,
            string replyToEmail = null)
        {
            try
            {
                fromEmail = string.IsNullOrEmpty(fromEmail) ? _fromEmail : fromEmail;


                var mmMsg = new MimeMessage();

                if (!string.IsNullOrEmpty(_fromEmail))
                {
                    mmMsg.From.Add(new MailboxAddress("Swapp Account", _fromEmail));
                }

                mmMsg.To.Add(new MailboxAddress(toEmail));
                mmMsg.Subject = subject;
                mmMsg.Body = new TextPart("plain")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_serverName, _port, _useSSL);

                    await client.AuthenticateAsync(_userName, _password);

                    await client.SendAsync(mmMsg);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Dispose()
        {
            _serverName = null;
            _fromEmail = null;
            _userName = null;
            _password = null;
            _host = null;
            _port = 0;
        }
    }
}
