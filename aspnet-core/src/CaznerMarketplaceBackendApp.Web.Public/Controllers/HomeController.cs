using Microsoft.AspNetCore.Mvc;
using CaznerMarketplaceBackendApp.Web.Controllers;

namespace CaznerMarketplaceBackendApp.Web.Public.Controllers
{
    public class HomeController : CaznerMarketplaceBackendAppControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}