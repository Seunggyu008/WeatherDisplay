using System;
using Microsoft.Extensions.Options;
using WeatherDisplay.Utils;
using System.Text.Json;
using WeatherDisplay.Models;
using WeatherDisplay.Models.WeatherDisplay;

namespace WeatherDisplay.Services.Weather
{
    //Loose coupling을 위한 인터페이스
    public interface IWeatherService
    {
        Task<WeatherResponseVM?> GetWeatherAsync(int nx, int ny);
    }

    public class WeatherDisplayService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherApiSettings _settings;
        private readonly ILogger<WeatherDisplayService> _logger;

        public WeatherDisplayService(IHttpClientFactory httpClientFactory, IOptions<WeatherApiSettings> options, ILogger<WeatherDisplayService> logger) {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
            _logger = logger;
        }

        //단기예보 api에 GET 요청을 하고, 받은 데이터를 WeatherReponseVM 객체로 변환합니다.
        public async Task<WeatherResponseVM> GetWeatherAsync (int nx, int ny)
        {
            var client = _httpClientFactory.CreateClient();
            var serviceKey = _settings.ServiceKey;
            var (baseDate, baseTime) = DateTimeCalculator.GetBaseDateTimeForForecast();

            var requestUrl = $"http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst?serviceKey={serviceKey}&dataType=JSON&numOfRows=1000&pageNo=1&base_date={baseDate}&base_time=0200&nx={nx}&ny={ny}";

            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherResponseVM>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            } else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API call failed with status code {StatusCode} and a content: {ErrorContent}", response.StatusCode, errorContent);
                return null;
            }
        }        
    }
}
