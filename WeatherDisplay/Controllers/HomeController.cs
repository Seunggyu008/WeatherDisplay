using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherDisplay.Models;
using System.Collections.Generic;
using WeatherDisplay.Models.Regions;

namespace WeatherDisplay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new RegionsVM
            {
                Regions = new List<RegionModel>
                {
                    new RegionModel { Name = "수도권", Latitude = 37.50, Longitude = 127.25 },
                    new RegionModel { Name = "강원도", Latitude = 37.80, Longitude = 128.20 },
                    new RegionModel { Name = "대전·세종·충남", Latitude = 36.54, Longitude = 127.15 },
                    new RegionModel { Name = "충북", Latitude = 36.97, Longitude = 127.72 },
                    new RegionModel { Name = "광주·전남", Latitude = 35.16, Longitude = 126.92 },
                    new RegionModel { Name = "전북", Latitude = 35.80, Longitude = 127.10 },
                    new RegionModel { Name = "대구·경북", Latitude = 36.19, Longitude = 128.40 },
                    new RegionModel { Name = "부산·울산·경남", Latitude = 35.31, Longitude = 128.83 },
                    new RegionModel { Name = "제주도", Latitude = 33.50, Longitude = 126.53 },
                    new RegionModel { Name = "흑산도·홍도", Latitude = 34.68, Longitude = 125.31 },
                    new RegionModel { Name = "독도", Latitude = 37.240, Longitude = 131.869 }
                }
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
