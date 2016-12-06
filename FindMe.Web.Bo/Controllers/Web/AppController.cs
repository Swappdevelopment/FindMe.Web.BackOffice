using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class AppController : BaseController
    {
        public AppController(IConfigurationRoot config)
            : base(config, null)
        {
        }



        public IActionResult Index()
        {
            return View();
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
