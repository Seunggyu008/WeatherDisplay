namespace WeatherDisplay.Utils
{
    public class DateTimeCalculator
    {
        //현재 시간에 가장 가까운 baseDate, baseTime을 계산해줍니다.
        //출저: gpt
        public static (string baseDate, string baseTime) GetBaseDateTimeForForecast()
        {
            // 기상청 예보 기준 시간대
            string[] baseTimes = { "2300", "0200", "0500", "0800", "1100", "1400", "1700", "2000" };
            var now = DateTime.UtcNow.AddHours(9); // KST

            foreach (var time in baseTimes.Reverse())
            {
                var baseDateTime = DateTime.ParseExact($"{now:yyyyMMdd} {time}", "yyyyMMdd HHmm", null);
                if (now >= baseDateTime.AddMinutes(10)) // 제공 시간 기준
                {
                    return (now.ToString("yyyyMMdd"), time);
                }
            }

            // 00:00~02:10 이전 → 전날 23:00 사용
            var yesterday = now.AddDays(-1);
            return (yesterday.ToString("yyyyMMdd"), "2300");
        }
    }
}
