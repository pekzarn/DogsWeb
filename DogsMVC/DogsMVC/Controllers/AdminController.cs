using Microsoft.AspNetCore.Mvc;

namespace DogsMVC.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("Admin");
        }
    }
}