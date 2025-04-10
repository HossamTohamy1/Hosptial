using Microsoft.AspNetCore.Mvc;

namespace Hospitel_Project.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
