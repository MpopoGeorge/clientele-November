namespace WeatherAPI.Domain.Entities
{
    public class WeatherInfo
    {
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"Weather in {City}, {Country}:\n" +
                   $"  Temperature: {Temperature}°C\n" +
                   $"  Description: {Description}\n" +
                   $"  Humidity: {Humidity}%\n" +
                   $"  Wind Speed: {WindSpeed} m/s\n" +
                   $"  Updated: {Timestamp:yyyy-MM-dd HH:mm:ss}";
        }
    }
}

