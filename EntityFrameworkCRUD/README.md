# Customer Management Console App

This is a simple .NET console application for managing customer data using Entity Framework Core, Serilog, and Bogus.

## Features

- View, add, update, delete customers
- Logs to `logs/app_log.txt` using Serilog
- Seeds fake customer data using Bogus
- Configurable number of seed customers in `appsettings.json`

## Configuration

Edit `appsettings.json`:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=customers.db"
  },
  "SeedSettings": {
    "GenerateCount": 20
  }
}
```

## Requirements

- .NET 6 or higher
- NuGet packages:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.Sqlite
  - Serilog.AspNetCore
  - Serilog.Sinks.File
  - Bogus

## Running the App

```bash
dotnet restore
dotnet run
```