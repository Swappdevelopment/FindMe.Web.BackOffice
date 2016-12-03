using Microsoft.AspNetCore.Mvc;

namespace FindMe.Web.App.Controllers
{
    [Route("")]
    [Route("[controller]")]
    [Route("[controller]/[action]")]
    public class PortalController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("About")]
        public PartialViewResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return PartialView();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
