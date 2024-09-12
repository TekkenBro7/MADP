using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253505_Sniazhko.Models;

namespace Web_253505_Sniazhko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Лабораторная работа 2";
            var model = new IndexViewModel(new List<ListDemo>()
            {
                new ListDemo { Id = 1, Name = "Пример 1" },
                new ListDemo { Id = 2, Name = "Пример 2" },
                new ListDemo { Id = 3, Name = "Пример 3" },
                new ListDemo { Id = 4, Name = "Пример 4" },
                new ListDemo { Id = 5, Name = "Пример 5" }
            });
            
            return View(model);
        }

        public IActionResult Lab1()
        {
            return View();
        }
    }
}
