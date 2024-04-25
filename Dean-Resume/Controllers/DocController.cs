using Microsoft.AspNetCore.Mvc;

namespace Dean_Resume.Controllers
{
    public class DocController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
