using Microsoft.AspNetCore.Mvc;

namespace DogsMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Start");
        }
    }
}