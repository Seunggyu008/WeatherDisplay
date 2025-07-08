using System;

namespace WeatherDisplay.Utils
{
    public class KmaGridConverter
    {
        public const int TO_GRID = 0;
        public const int TO_GPS = 1;

        //위도, 경도를 격자 좌표로 변환해주는 유틸 클래스입니다
        //출저: gpt
        public Point Convert(double lat, double lon)
        {
            double RE = 6371.00877; // 지구 반경(km)
            double GRID = 5.0; // 격자 간격(km)
            double SLAT1 = 30.0; // 투영 위도1(degree)
            double SLAT2 = 60.0; // 투영 위도2(degree)
            double OLON = 126.0; // 기준점 경도(degree)
            double OLAT = 38.0; // 기준점 위도(degree)
            double XO = 43; // 기준점 X좌표(GRID)
            double YO = 136; // 기_준점 Y좌표(GRID)

            double DEGRAD = Math.PI / 180.0;
            double RADDEG = 180.0 / Math.PI;

            double re = RE / GRID;
            double slat1 = SLAT1 * DEGRAD;
            double slat2 = SLAT2 * DEGRAD;
            double olon = OLON * DEGRAD;
            double olat = OLAT * DEGRAD;

            double sn = Math.Tan(Math.PI * 0.25 + slat2 * 0.5) / Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sn = Math.Log(Math.Cos(slat1) / Math.Cos(slat2)) / Math.Log(sn);
            double sf = Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sf = Math.Pow(sf, sn) * Math.Cos(slat1) / sn;
            double ro = Math.Tan(Math.PI * 0.25 + olat * 0.5);
            ro = re * sf / Math.Pow(ro, sn);
            
            Point pt = new Point();

            pt.Lat = lat;
            pt.Lon = lon;
            double ra = Math.Tan(Math.PI * 0.25 + (lat) * DEGRAD * 0.5);
            ra = re * sf / Math.Pow(ra, sn);
            double theta = lon * DEGRAD - olon;
            if (theta > Math.PI) theta -= 2.0 * Math.PI;
            if (theta < -Math.PI) theta += 2.0 * Math.PI;
            theta *= sn;
            pt.X = (int)Math.Floor(ra * Math.Sin(theta) + XO + 0.5);
            pt.Y = (int)Math.Floor(ro - ra * Math.Cos(theta) + YO + 0.5);
            
            return pt;
        }

        public class Point
        {
            public double Lat { get; set; }
            public double Lon { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}