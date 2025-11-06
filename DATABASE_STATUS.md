# Database Status Report

## Overview

This document describes the database configuration and status for all projects in the Clientele repository.

---

## Projects with Databases

### 1. EntityFrameworkCRUD ✅

**Database Provider**: SQL Server  
**Database Name**: `CustomerDb`  
**Connection String**: `Server=.;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;`

**Configuration**:
- **Location**: `EntityFrameworkCRUD/appsettings.json`
- **DbContext**: `CustomerDbContext`
- **Entity**: `Customer` (CustomerId, Name, Email, PhoneNumber)
- **Migrations**: Yes (Initial migration exists)

**Database Setup**:
- Uses `EnsureCreated()` method in `Program.cs` to automatically create the database
- Seeds 20 fake customers using Bogus on first run
- Database is created automatically when the application runs

**How It Works**:
```csharp
// In Program.cs
var dbContext = serviceProvider.GetRequiredService<CustomerDbContext>();
dbContext.Database.EnsureCreated(); // Creates database if it doesn't exist
SeedData(dbContext, logger, seedSettings); // Seeds initial data
```

**Requirements**:
- SQL Server (LocalDB, Express, or Full SQL Server)
- SQL Server must be running
- Windows Authentication (Trusted_Connection=True)
- Database will be created automatically on first run

**Status**: 
- ✅ Database configuration is correct
- ✅ Database will be created automatically on first run
- ⚠️ Requires SQL Server to be installed and running

**To Check Database Status**:
```bash
cd EntityFrameworkCRUD
dotnet run
# Database will be created automatically if it doesn't exist
```

**To Manually Create Database**:
```bash
cd EntityFrameworkCRUD
dotnet ef database update
```

**To View Database**:
- Use SQL Server Management Studio (SSMS)
- Connect to: `Server=.;Database=CustomerDb;Trusted_Connection=True;`
- Or use Azure Data Studio

---

## Projects WITHOUT Databases

### 2. ProductAPI ✅

**Storage Type**: In-Memory (List<Product>)  
**Repository**: `InMemoryProductRepository`  
**Persistence**: Data is lost when application stops

**Configuration**:
- **Location**: `ProductAPI/Infrastructure/Repositories/InMemoryProductRepository.cs`
- **Storage**: `List<Product>` in memory
- **Initial Data**: Pre-populated with sample products

**How It Works**:
```csharp
// In InMemoryProductRepository.cs
private readonly List<Product> _products;

public InMemoryProductRepository()
{
    _products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", ... },
        new Product { Id = 2, Name = "Mouse", ... },
        // ... more products
    };
}
```

**Status**: 
- ✅ No database required
- ✅ Works out of the box
- ⚠️ Data is not persisted (lost on restart)

**Note**: For production use, consider implementing a database-backed repository (Entity Framework Core, etc.)

---

## Other Projects

### 3. LibraryManagementSystem
**Storage**: In-Memory (List<Book>)  
**Status**: No database required ✅

### 4. FactorialCalculator
**Storage**: None (calculations only)  
**Status**: No database required ✅

### 5. ProducerConsumerPattern
**Storage**: In-Memory (ConcurrentQueue)  
**Status**: No database required ✅

### 6. WeatherAPI
**Storage**: None (API calls only)  
**Status**: No database required ✅

---

## Database Requirements Summary

| Project | Database Type | Status | Requires SQL Server |
|---------|--------------|--------|---------------------|
| **EntityFrameworkCRUD** | SQL Server | ✅ Configured | ✅ Yes |
| **ProductAPI** | In-Memory | ✅ Working | ❌ No |
| **LibraryManagementSystem** | In-Memory | ✅ Working | ❌ No |
| **FactorialCalculator** | None | ✅ Working | ❌ No |
| **ProducerConsumerPattern** | In-Memory | ✅ Working | ❌ No |
| **WeatherAPI** | None | ✅ Working | ❌ No |

---

## SQL Server Setup Instructions

### For EntityFrameworkCRUD:

1. **Install SQL Server** (if not already installed):
   - SQL Server Express (free): https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Or use SQL Server LocalDB (included with Visual Studio)

2. **Verify SQL Server is Running**:
   ```bash
   # Windows PowerShell
   Get-Service -Name "MSSQLSERVER" | Select-Object Status, Name
   
   # Or check SQL Server Configuration Manager
   ```

3. **Update Connection String** (if needed):
   - Edit `EntityFrameworkCRUD/appsettings.json`
   - Change `Server=.` to your SQL Server instance name if different
   - Example: `Server=localhost\SQLEXPRESS;Database=CustomerDb;...`

4. **Run the Application**:
   ```bash
   cd EntityFrameworkCRUD
   dotnet run
   ```
   - Database will be created automatically
   - Sample data will be seeded

5. **Verify Database Creation**:
   - Open SQL Server Management Studio
   - Connect to your SQL Server instance
   - Look for `CustomerDb` database
   - Check `Customers` table

---

## Troubleshooting

### EntityFrameworkCRUD Database Issues:

1. **"Cannot open database" error**:
   - Ensure SQL Server is running
   - Check connection string in `appsettings.json`
   - Verify Windows Authentication is enabled

2. **"Login failed" error**:
   - Check SQL Server authentication settings
   - Try using SQL Server Authentication instead:
     ```json
     "DefaultConnection": "Server=.;Database=CustomerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
     ```

3. **"Database does not exist"**:
   - This is normal - database will be created automatically
   - Run the application and it will create the database

4. **"Pending model changes" warning**:
   - Run: `dotnet ef migrations add MigrationName`
   - Then: `dotnet ef database update`

### ProductAPI (In-Memory):

- **Data lost on restart**: This is expected behavior
- **To persist data**: Implement a database-backed repository

---

## Migration Commands

### For EntityFrameworkCRUD:

```bash
cd EntityFrameworkCRUD

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# List all migrations
dotnet ef migrations list

# Get database context info
dotnet ef dbcontext info
```

---

## Current Status

✅ **EntityFrameworkCRUD**: Database configured, will be created automatically  
✅ **ProductAPI**: In-memory storage, no database required  
✅ **All other projects**: No database required  

⚠️ **Note**: EntityFrameworkCRUD requires SQL Server to be installed and running

---

## Recommendations

1. **For Development**:
   - Use SQL Server LocalDB or Express for EntityFrameworkCRUD
   - In-memory storage is fine for other projects

2. **For Production**:
   - Consider migrating ProductAPI to use a database (EF Core)
   - Use proper SQL Server instance for EntityFrameworkCRUD
   - Implement proper connection string management (secrets, environment variables)

3. **For Testing**:
   - Use In-Memory database provider for EntityFrameworkCRUD tests
   - Already implemented in test projects ✅

---

## Next Steps

1. ✅ Verify SQL Server is running (for EntityFrameworkCRUD)
2. ✅ Run EntityFrameworkCRUD to create database automatically
3. ✅ Test all projects to ensure databases/storage work correctly
4. ⚠️ Consider adding database persistence to ProductAPI for production use

