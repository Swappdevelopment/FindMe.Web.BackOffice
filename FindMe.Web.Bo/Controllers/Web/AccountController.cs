using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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


        public async Task<IActionResult> Profile()
        {
            return await CheckForAccess(
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


        public async Task<IActionResult> ValidateEmail(string id = null)
        {
            bool success = true;

            try
            {
                WebRootPath();
                await _repo.Execute("ValidateEmailTokenExists", id);
            }
            catch (Exception ex)
            {
                this.LogCritical(ex);
                success = false;
            }

            return View(success);
        }


        public IActionResult SignUp()
        {
            return View();
        }
    }
}
