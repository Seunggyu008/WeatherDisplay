namespace WeatherDisplay.Models.WeatherDisplay
{
    public class WeatherDataVM
    {
        //하늘상태
        public int SkyStatus { get; set; }

        //일 최고 기온
        public int MaxTemp { get; set; }

        //일 최저 기온
        public int MinTemp { get; set; }

        //강수확률
        public int Precipitation { get; set; }

        //습도
        public int Wetness { get; set; }

        public List<RegionModel> Regions { get; set; }
    }
}
