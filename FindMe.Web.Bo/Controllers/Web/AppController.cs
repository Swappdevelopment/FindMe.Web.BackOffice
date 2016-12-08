using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class AppController : BaseController
    {
        public AppController(
            IConfigurationRoot config,
            IHostingEnvironment env)
            : base(config, null, env, null)
        {
        }


        public IActionResult Index()
        {
            return CheckForAccess(
                AccessLevel.CookieSignedIn,
                () =>
                {
                    WebRootPath();

                    return View();
                });
        }

        public PartialViewResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return PartialView();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
