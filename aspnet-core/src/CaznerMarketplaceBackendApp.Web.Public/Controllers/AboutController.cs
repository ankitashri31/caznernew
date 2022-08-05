using Microsoft.AspNetCore.Mvc;
using CaznerMarketplaceBackendApp.Web.Controllers;

namespace CaznerMarketplaceBackendApp.Web.Public.Controllers
{
    public class AboutController : CaznerMarketplaceBackendAppControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}