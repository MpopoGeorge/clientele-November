# Clientele Assessments

This repository contains 6 C# projects implementing various programming concepts and best practices. All projects follow Clean Architecture principles where applicable.

## Projects Overview

### 1. Library Management System ✅
**Location**: `LibraryManagementSystem/`

A simple library management system that allows adding, removing, and searching for books. Each book has properties like Title, Author, ISBN, and Publication Year.

**Features**:
- Add books to the library
- Remove books by ISBN
- Search books by title, author, or ISBN
- Display all books
- Clean Architecture implementation

**Run**: `cd LibraryManagementSystem/LibraryManagementSystem && dotnet run`

---

### 2. Factorial Calculator ✅
**Location**: `FactorialCalculator/`

A C# program that creates multiple threads to calculate the factorial of different numbers concurrently. Ensures thread safety and proper synchronization.

**Features**:
- Multi-threaded factorial calculations
- Thread-safe operations using locks
- Parallel processing with Tasks
- Parallel.ForEach implementation

**Run**: `cd FactorialCalculator/FactorialCalculator && dotnet run`

---

### 3. Producer-Consumer Pattern ✅
**Location**: `ProducerConsumerPattern/`

Implements a producer-consumer scenario using a Queue. The producer generates random numbers and adds them to the queue, while the consumer reads and prints these numbers.

**Features**:
- Multiple producers and consumers
- Thread-safe ConcurrentQueue
- Graceful shutdown mechanism
- Queue size management

**Run**: `cd ProducerConsumerPattern/ProducerConsumerPattern && dotnet run`

---

### 4. Entity Framework CRUD ✅
**Location**: `EntityFrameworkCRUD/`

A C# application that connects to a SQL database using Entity Framework. Implements CRUD operations for a Customer entity with properties like CustomerID, Name, Email, and PhoneNumber.

**Features**:
- Full CRUD operations (Create, Read, Update, Delete)
- Entity Framework Core with SQL Server
- Dependency Injection
- Logging with Serilog
- Data seeding with Bogus
- Clean Architecture implementation

**Run**: 
1. Update `appsettings.json` with your connection string
2. `cd EntityFrameworkCRUD && dotnet run`

---

### 5. Product API (RESTful API) ✅
**Location**: `ProductAPI/`

A RESTful API built with ASP.NET Core that manages a collection of Products. The API supports operations to create, read, update, and delete products. Each product has properties like ProductID, Name, Description, and Price.

**Features**:
- RESTful API endpoints
- Full CRUD operations
- Swagger/OpenAPI documentation
- Clean Architecture implementation
- Input validation and error handling

**Run**: 
1. `cd ProductAPI && dotnet run`
2. Navigate to `https://localhost:5001/swagger` for API documentation

**API Endpoints**:
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

---

### 6. Weather API Consumer ✅
**Location**: `WeatherAPI/`

A C# console application that consumes a public REST API (OpenWeatherMap API). The application fetches and displays the current weather information for a given city.

**Features**:
- Consumes OpenWeatherMap REST API
- Displays weather information (temperature, description, humidity, wind speed)
- Error handling and validation
- Clean Architecture implementation

**Run**: 
1. Get a free API key from https://openweathermap.org/api
2. Set environment variable: `set OPENWEATHER_API_KEY=your_key` (Windows) or `export OPENWEATHER_API_KEY=your_key` (Linux/Mac)
3. `cd WeatherAPI && dotnet run`

---

## Architecture

All projects follow **Clean Architecture** principles where applicable:

- **Domain Layer**: Core business entities
- **Application Layer**: Business logic, interfaces, and services
- **Infrastructure Layer**: External dependencies (databases, APIs, etc.)
- **Presentation Layer**: User interface (Console, Web API)

### Benefits:
- Separation of concerns
- Dependency inversion
- Testability
- Maintainability
- Scalability

---

## Prerequisites

- .NET 8.0 SDK or higher
- SQL Server (for EntityFrameworkCRUD project)
- OpenWeatherMap API key (for WeatherAPI project - free tier available)

---

## Running All Projects

Each project can be run independently:

```bash
# Library Management System
cd LibraryManagementSystem/LibraryManagementSystem
dotnet run

# Factorial Calculator
cd FactorialCalculator/FactorialCalculator
dotnet run

# Producer-Consumer Pattern
cd ProducerConsumerPattern/ProducerConsumerPattern
dotnet run

# Entity Framework CRUD
cd EntityFrameworkCRUD
dotnet run

# Product API
cd ProductAPI
dotnet run

# Weather API Consumer
cd WeatherAPI
dotnet run
```

---

## Code Quality

- ✅ Clean, well-documented code
- ✅ Follows best practices
- ✅ Clean Architecture implementation
- ✅ Proper error handling
- ✅ Input validation
- ✅ Dependency Injection where applicable

---

## Testing

All projects have comprehensive NUnit test suites:

### Test Projects

1. **EntityFrameworkCRUD.Tests** - 13 tests
   - CustomerService tests with Moq mocking
   - CRUD operations, validation, error handling

2. **ProductAPI.Tests** - 11 tests
   - ProductService tests with Moq mocking
   - CRUD operations, validation, error handling

3. **LibraryManagementSystem.Tests** - 13 tests
   - LibraryService tests
   - Add, remove, search operations, validation

4. **WeatherAPI.Tests** - 5 tests
   - WeatherService tests with HttpClient mocking
   - API calls, error handling

5. **FactorialCalculator.Tests** - 7 tests
   - Factorial calculation logic
   - Edge cases, thread safety

6. **ProducerConsumerPattern.Tests** - 5 tests
   - ConcurrentQueue operations
   - Thread safety tests

**Total: 54 tests, all passing ✅**

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for a specific project
dotnet test EntityFrameworkCRUD.Tests
dotnet test ProductAPI.Tests
dotnet test LibraryManagementSystem.Tests
dotnet test WeatherAPI.Tests
dotnet test FactorialCalculator.Tests
dotnet test ProducerConsumerPattern.Tests
```

The architecture supports easy unit testing through interfaces and dependency injection.

---

## Repository

This code is available on GitHub: https://github.com/MpopoGeorge/clientele-November.git

---

## Author

George Mpopo <gtmpopo@gmail.com>

---

## License

This project is for Clientele Assessment purposes.
