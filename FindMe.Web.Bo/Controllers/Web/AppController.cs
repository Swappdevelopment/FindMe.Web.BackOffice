using FindMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindMe.Web.App
{
    public class AppController : BaseController
    {
        public AppController(IConfigurationRoot config, AppDbInteractor dbi)
            : base(config, dbi)
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
