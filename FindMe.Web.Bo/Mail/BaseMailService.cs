﻿using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class BaseMailService : IMailService
    {
        protected string _baseEmailServerName;
        protected string _baseEmail;
        protected string _baseEmailPassword;

        public BaseMailService()
        {
            _baseEmailServerName = null;
            _baseEmail = null;
            _baseEmailPassword = null;
        }


        public virtual async Task SendEmailAsync(string email, string subject, string message)
        {
            var mmMsg = new MimeMessage();
            mmMsg.From.Add(new MailboxAddress("Swapp Account", _baseEmail));
            mmMsg.To.Add(new MailboxAddress(email));
            mmMsg.Subject = subject;
            mmMsg.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_baseEmailServerName, 587, false);

                await client.AuthenticateAsync(_baseEmail, _baseEmailPassword);

                //client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
                await client.SendAsync(mmMsg);
                await client.DisconnectAsync(true);
            }
        }

        public virtual void Dispose()
        {
            _baseEmailServerName = null;
            _baseEmail = null;
            _baseEmailPassword = null;
        }
    }
}