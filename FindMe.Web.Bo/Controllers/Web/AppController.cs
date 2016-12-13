using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class AppController : BaseController
    {
        public AppController(
            IConfigurationRoot config,
            WebDbRepository repo,
            IHostingEnvironment env)
            : base(config, repo, env, null, null)
        {
        }


        public async Task<IActionResult> Index()
        {
            return await CheckForAccess(
                AccessLevel.DbSignedIn,
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
