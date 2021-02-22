using System.Web.Mvc;

namespace Web.Controllers
{
    public class UrlController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}