using Newtonsoft.Json;

namespace WeatherDisplay.Models.WeatherDisplay
{
    public class WeatherParamVM
    {
        [JsonProperty("baseDate")]
        public string BaseDate { get; set; }

        [JsonProperty("baseTime")]
        public string BaseTime { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("fcstDate")]
        public string FcstDate { get; set; }

        [JsonProperty("fcstTime")]
        public string FcstTime { get; set; }

        [JsonProperty("fcstValue")]
        public string FcstValue { get; set; }

        [JsonProperty("nx")]
        public int Nx { get; set; }

        [JsonProperty("ny")]
        public int Ny { get; set; }
    }
}
