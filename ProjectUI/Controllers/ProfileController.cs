using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
