using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public interface IMailService : IDisposable
    {
        Task SendEmailAsync(string subject, string message, string toEmail = null, string fromEmail = null, string replyToEmail = null);
    }
}
