using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class AccountController : BaseController
    {
        public AccountController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env)
            : base(config, repo, env, null, null)
        {
        }


        public IActionResult SignIn(string redirectUrl = "")
        {
            WebRootPath();
            ViewBag.RedirectUrl = string.IsNullOrEmpty(redirectUrl) ? this.Url.Action("Index", "App") : redirectUrl;

            return View();
        }


        public IActionResult Profile()
        {
            return CheckForAccess(
                AccessLevel.CookieSignedIn,
                () =>
                {
                    WebRootPath();
                    return View();
                });
        }


        public async Task<IActionResult> SignOut()
        {
            await _repo.Execute("SignOut");

            this.RemoveSignedCookies();

            return RedirectToAction("SignIn", "Account");
        }



        public IActionResult SignUp()
        {
            return View();
        }
    }
}
