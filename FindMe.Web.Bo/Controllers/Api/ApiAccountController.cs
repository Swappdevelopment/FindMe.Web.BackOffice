using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class ApiAccountController : BaseController
    {
        public ApiAccountController(IConfigurationRoot config, AppDbInteractor dbi)
            : base(config, dbi)
        {
        }


        [HttpPost]
        public IActionResult SignIn([FromBody]dynamic signInVm)
        {
            string userName = signInVm.userName;
            string password = signInVm.password;
            bool remember = signInVm.remember;

            return Ok("good");
        }



        public IActionResult SignUp()
        {
            return View();
        }
    }
}
