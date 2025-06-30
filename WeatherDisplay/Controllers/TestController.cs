using Microsoft.AspNetCore.Mvc;

namespace WeatherDisplay.Controllers
{
    public class TestController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.Message = "Hello from index";
            return View();
        }

        public IActionResult Details(int id) => View(id);

        public IActionResult GetData() => Json(new { message = "Hello this is an experiemnt" });

        [HttpPost]
        public IActionResult ProcessForm(string name)
        {
            ViewBag.Message = $"Hello {name}";
            return View("Index");
        }
    }
}
