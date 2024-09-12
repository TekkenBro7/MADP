using Microsoft.AspNetCore.Mvc;

namespace Web_253505_Sniazhko.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}