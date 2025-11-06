# Weather API Consumer

A C# console application that consumes the OpenWeatherMap API to fetch and display current weather information for a given city. This project follows Clean Architecture principles.

## Features

- Fetch current weather information for any city
- Display temperature, description, humidity, and wind speed
- Clean Architecture with separation of concerns
- Error handling and validation

## Architecture

This project follows Clean Architecture with the following layers:

- **Domain**: WeatherInfo entity
- **Application**: IWeatherService interface and WeatherService implementation
- **Infrastructure**: HTTP client for API calls
- **Presentation**: Console UI

## Prerequisites

- .NET 8.0 SDK or higher
- OpenWeatherMap API key (free at https://openweathermap.org/api)

## Getting an API Key

1. Visit https://openweathermap.org/api
2. Sign up for a free account
3. Get your API key from the dashboard
4. Set it as an environment variable or update the code

## Configuration

### Option 1: Environment Variable (Recommended)
```bash
# Windows
set OPENWEATHER_API_KEY=your_api_key_here

# Linux/Mac
export OPENWEATHER_API_KEY=your_api_key_here
```

### Option 2: Update Code
Edit `WeatherAPI/Presentation/Program.cs` and replace `"YOUR_API_KEY_HERE"` with your actual API key.

## Running the Application

1. Navigate to the WeatherAPI directory:
```bash
cd WeatherAPI
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Set your API key (if using environment variable):
```bash
# Windows
set OPENWEATHER_API_KEY=your_api_key_here

# Linux/Mac
export OPENWEATHER_API_KEY=your_api_key_here
```

4. Run the application:
```bash
dotnet run
```

5. Enter a city name when prompted, or type 'exit' to quit.

## Example Usage

```
=== Weather API Consumer ===
Enter a city name to get weather information (or 'exit' to quit)

Enter city name: London

Fetching weather for London...

Weather in London, GB:
  Temperature: 15.5°C
  Description: clear sky
  Humidity: 65%
  Wind Speed: 3.2 m/s
  Updated: 2025-11-06 12:30:45

Enter city name: New York

Fetching weather for New York...

Weather in New York, US:
  Temperature: 22.3°C
  Description: partly cloudy
  Humidity: 70%
  Wind Speed: 4.5 m/s
  Updated: 2025-11-06 12:31:20

Enter city name: exit
Goodbye!
```

## Error Handling

The application handles various error scenarios:
- Invalid city names
- Network connectivity issues
- API rate limits
- Missing API key


