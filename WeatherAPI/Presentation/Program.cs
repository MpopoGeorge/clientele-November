using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Application.Services;

var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole());
services.AddHttpClient();
services.AddScoped<IWeatherService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    
    var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY") 
                 ?? "YOUR_API_KEY_HERE";
    
    if (apiKey == "YOUR_API_KEY_HERE")
    {
        Console.WriteLine("WARNING: Please set your OpenWeatherMap API key!");
        Console.WriteLine("You can get a free API key from: https://openweathermap.org/api");
        Console.WriteLine("Set it as an environment variable: OPENWEATHER_API_KEY");
        Console.WriteLine("Or update the code with your API key.\n");
    }
    
    return new WeatherService(httpClient, apiKey);
});

var serviceProvider = services.BuildServiceProvider();
var weatherService = serviceProvider.GetRequiredService<IWeatherService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

Console.WriteLine("=== Weather API Consumer ===");
Console.WriteLine("Enter a city name to get weather information (or 'exit' to quit)\n");

bool running = true;

while (running)
{
    try
    {
        Console.Write("Enter city name: ");
        string? cityName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(cityName))
        {
            Console.WriteLine("City name cannot be empty. Please try again.\n");
            continue;
        }

        if (cityName.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            running = false;
            Console.WriteLine("Goodbye!");
            continue;
        }

        Console.WriteLine($"\nFetching weather for {cityName}...\n");

        var weatherInfo = await weatherService.GetWeatherByCityAsync(cityName);

        if (weatherInfo != null)
        {
            Console.WriteLine(weatherInfo);
        }
        else
        {
            Console.WriteLine($"Weather information not available for {cityName}");
        }

        Console.WriteLine();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error fetching weather data");
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}

