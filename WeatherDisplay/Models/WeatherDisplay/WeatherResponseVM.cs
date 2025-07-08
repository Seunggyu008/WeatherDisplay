namespace WeatherDisplay.Models.WeatherDisplay
{
    public class WeatherResponseVM 
    {
        public Response Response { get; set; }
    }
    
    public class Response
    {
        public Header Header { get; set; }
        public Body Body { get; set; }
    }

    public class Header
    {
        public string ResultCode { get; set; }
        public string ResultMsg { get; set; }

    }

    public class  Body
    {
        public string DataType { get; set; }
        public Items Items { get; set; }
    }

    public class Items
    {
        public List<WeatherParamVM> Item { get; set; }
    }
}
