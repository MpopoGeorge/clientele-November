# Entity Framework CRUD

Console app for managing customers with Entity Framework Core and SQL Server.

## Features

- CRUD operations for customers
- SQL Server database
- Data seeding with South African names and phone numbers
- Logging with Serilog

## Setup

1. Ensure SQL Server is running
2. Update connection string in `appsettings.json` if needed
3. Run: `dotnet run`

Database is created automatically on first run.

## Configuration

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "SeedSettings": {
    "GenerateCount": 20
  }
}
```
