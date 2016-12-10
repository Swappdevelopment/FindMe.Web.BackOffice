using System;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public interface IMailService : IDisposable
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
