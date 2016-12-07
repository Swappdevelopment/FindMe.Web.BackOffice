using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FindMe.Web.App
{
    public class AppController : BaseController
    {
        public AppController(IConfigurationRoot config)
            : base(config, null, null)
        {
        }


        public IActionResult Index()
        {
            var result = CheckForRedirection(AccessLevel.CookieSignedIn);

            if(result == null)
            {
                result = View();
            }

            return result;
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
