using System.Text.Json;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Domain.Entities;

namespace WeatherAPI.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<WeatherInfo?> GetWeatherByCityAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                throw new ArgumentException("City name cannot be empty", nameof(cityName));

            try
            {
                var url = $"{_baseUrl}?q={Uri.EscapeDataString(cityName)}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        throw new Exception($"City '{cityName}' not found");
                    
                    throw new Exception($"Failed to fetch weather data. Status: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (weatherData == null)
                    return null;

                return new WeatherInfo
                {
                    City = weatherData.Name,
                    Country = weatherData.Sys?.Country ?? "",
                    Temperature = weatherData.Main?.Temp ?? 0,
                    Description = weatherData.Weather?.FirstOrDefault()?.Description ?? "",
                    Humidity = weatherData.Main?.Humidity ?? 0,
                    WindSpeed = weatherData.Wind?.Speed ?? 0,
                    Timestamp = DateTime.Now
                };
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error connecting to weather API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error parsing weather data: {ex.Message}", ex);
            }
        }

        private class OpenWeatherResponse
        {
            public string Name { get; set; } = string.Empty;
            public MainData? Main { get; set; }
            public WindData? Wind { get; set; }
            public List<WeatherData>? Weather { get; set; }
            public SysData? Sys { get; set; }
        }

        private class MainData
        {
            public double Temp { get; set; }
            public double Humidity { get; set; }
        }

        private class WindData
        {
            public double Speed { get; set; }
        }

        private class WeatherData
        {
            public string Description { get; set; } = string.Empty;
        }

        private class SysData
        {
            public string Country { get; set; } = string.Empty;
        }
    }
}

