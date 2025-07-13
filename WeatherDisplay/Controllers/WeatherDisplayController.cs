using Microsoft.AspNetCore.Mvc;
using WeatherDisplay.Services.Weather;
using WeatherDisplay.Utils;

namespace WeatherDisplay.Controllers
{
    public class WeatherDisplayController : Controller
    {
        private readonly IWeatherService _weatherDisplayService;

        public WeatherDisplayController(IWeatherService weatherDisplayService)
        {
            _weatherDisplayService = weatherDisplayService;
        }

        public async Task<IActionResult> GetWeatherData(double latitude, double longitude)
        {
            var converter = new KmaGridConverter();
            var gridPoint = converter.Convert(latitude, longitude);

            var weatherResponse = await _weatherDisplayService.GetWeatherAsync(gridPoint.X, gridPoint.Y);

            if (weatherResponse == null)
            {
                ViewData["ErrorMessage"] = "Failed to retrieve weather data. Please try again later.";
                return View("~/Views/Weather/WeatherDisplay.cshtml");
            }

            var weatherData = new WeatherDataVM();


            if (weatherResponse?.Response?.Body?.Items?.Item != null)
            {
                var items = weatherResponse.Response.Body.Items.Item;

                var tmxItem = items.Where(i => i.Category == "TMX").OrderByDescending(i => i.FcstValue).FirstOrDefault();
                var tmnItem = items.Where(i => i.Category == "TMN").OrderBy(i => i.FcstValue).FirstOrDefault();

                weatherData.SkyStatus = int.Parse(items.FirstOrDefault(i => i.Category == "SKY")?.FcstValue ?? "0");
                weatherData.MaxTemp = tmxItem != null ? (int)float.Parse(tmxItem.FcstValue) : 0;
                weatherData.MinTemp = tmnItem != null ? (int)float.Parse(tmnItem.FcstValue) : 0;
                weatherData.Precipitation = int.Parse(items.FirstOrDefault(i => i.Category == "POP")?.FcstValue ?? "0");
                weatherData.Wetness = int.Parse(items.FirstOrDefault(i => i.Category == "REH")?.FcstValue ?? "0");
            }

            return View("~/Views/Weather/WeatherDisplay.cshtml", weatherData);
        }
    }
}
