 using System.Web.Mvc;
 using ViewModels;


namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("login", "Account");
        }
    
    }


}



