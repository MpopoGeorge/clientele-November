using WeatherAPI.Domain.Entities;

namespace WeatherAPI.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherInfo?> GetWeatherByCityAsync(string cityName);
    }
}

