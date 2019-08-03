using System.Web.Mvc;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("/Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
