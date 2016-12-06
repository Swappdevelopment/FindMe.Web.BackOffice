using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class AccountController : BaseController
    {
        public AccountController(IConfigurationRoot config)
            : base(config, null)
        {
        }



        public IActionResult SignIn()
        {
            return View();
        }



        public IActionResult SignUp()
        {
            return View();
        }
    }
}
