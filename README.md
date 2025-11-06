# Clientele Assessments

6 C# projects for technical assessment.

## Projects

### 1. Library Management System
Console app for managing books. Add, remove, search by title/author/ISBN.

Run: `cd LibraryManagementSystem/LibraryManagementSystem && dotnet run`

### 2. Factorial Calculator
Multi-threaded factorial calculator using Threads, Tasks, and Parallel.ForEach.

Run: `cd FactorialCalculator/FactorialCalculator && dotnet run`

### 3. Producer-Consumer Pattern
Producer-consumer demo with ConcurrentQueue. Multiple producers and consumers.

Run: `cd ProducerConsumerPattern/ProducerConsumerPattern && dotnet run`

### 4. Entity Framework CRUD
Console app with Entity Framework Core and SQL Server. CRUD operations for Customer entity.

Run: `cd EntityFrameworkCRUD && dotnet run`

Requires SQL Server. Database created automatically on first run.

### 5. Product API
RESTful API for product management. Swagger UI available.

Run: `cd ProductAPI && dotnet run`

Swagger: `http://localhost:5000/swagger`

### 6. Weather API Consumer
Console app that fetches weather data from OpenWeatherMap API.

Run: `cd WeatherAPI && dotnet run`

Requires OpenWeatherMap API key. Set environment variable: `OPENWEATHER_API_KEY`

## Testing

Run all tests: `dotnet test`

54 tests total, all passing.

## Requirements

- .NET 9.0 SDK
- SQL Server (for EntityFrameworkCRUD)
- OpenWeatherMap API key (for WeatherAPI)

## Author

George Mpopo <gtmpopo@gmail.com>
