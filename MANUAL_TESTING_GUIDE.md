# Manual Testing Guide

## Projects Run Sequentially ✅

All 6 projects have been run successfully. Here's how to test each one manually:

---

## 1. LibraryManagementSystem ✅

**Status**: Running successfully  
**Type**: Interactive Console Application

**How to Test**:
```bash
cd LibraryManagementSystem/LibraryManagementSystem
dotnet run
```

**Test Steps**:
1. Select option 1 to add a new book
2. Enter book details (title, author, ISBN)
3. Select option 3 to search by title
4. Select option 4 to search by author
5. Select option 5 to search by ISBN
6. Select option 6 to display all books
7. Select option 2 to remove a book
8. Select option 7 to exit

**Expected Output**: Menu-driven interface with book management operations

---

## 2. FactorialCalculator ✅

**Status**: Running successfully  
**Type**: Console Application (runs automatically)

**How to Test**:
```bash
cd FactorialCalculator/FactorialCalculator
dotnet run
```

**Test Steps**:
- The application runs automatically and calculates factorials using:
  - Multiple threads
  - Tasks
  - Parallel.ForEach

**Expected Output**:
- Factorial calculations for numbers: 5, 10, 15, 20, 25, 30
- Results showing thread-safe calculations
- All three methods (Threads, Tasks, Parallel.ForEach) produce correct results

**Sample Output**:
```
Factorial of 5 = 120
Factorial of 10 = 3628800
Factorial of 20 = 2432902008176640000
...
```

---

## 3. ProducerConsumerPattern ✅

**Status**: Running successfully  
**Type**: Console Application (runs continuously)

**How to Test**:
```bash
cd ProducerConsumerPattern/ProducerConsumerPattern
dotnet run
```

**Test Steps**:
- The application runs automatically
- Press any key to stop when done testing

**Expected Output**:
- Multiple producers adding items to a queue
- Multiple consumers removing items from the queue
- Queue size updates showing thread-safe operations
- "Queue is full" messages when capacity is reached
- "Queue is empty" messages when consumers wait

**Sample Output**:
```
Producer 1 added: 95 (Queue size: 1)
Consumer 1 consumed: 95 (Queue size: 0)
Queue is full. Producer 2 waiting...
```

---

## 4. EntityFrameworkCRUD ✅

**Status**: Running successfully  
**Type**: Console Application

**How to Test**:
```bash
cd EntityFrameworkCRUD
dotnet run
```

**Test Steps**:
1. The application will create a database and seed sample data
2. It will perform CRUD operations on Customer entities
3. Check the output for:
   - Database creation messages
   - Customer creation, reading, updating, and deletion
   - Logging output

**Expected Output**:
- Database initialization messages
- Customer CRUD operations
- Logging information (if configured)

**Note**: Ensure SQL Server is running or update connection string in `appsettings.json`

---

## 5. ProductAPI ✅

**Status**: Running successfully  
**Type**: ASP.NET Core Web API

**How to Test**:
```bash
cd ProductAPI
dotnet run
```

**API Endpoints**:
- **Base URL**: `http://localhost:5159` (check launchSettings.json for actual port)
- **Swagger UI**: `http://localhost:5159/swagger`

**Test Steps**:

1. **Start the API**:
   ```bash
   dotnet run
   ```

2. **Open Swagger UI**:
   - Navigate to `http://localhost:5159/swagger`
   - You'll see all available endpoints

3. **Test Endpoints**:

   **GET /api/products**
   - Get all products
   - Should return empty array initially

   **POST /api/products**
   - Create a new product
   - Body example:
     ```json
     {
       "name": "Test Product",
       "description": "Test Description",
       "price": 99.99,
       "stock": 100
     }
     ```

   **GET /api/products/{id}**
   - Get product by ID
   - Use the ID from the POST response

   **PUT /api/products/{id}**
   - Update a product
   - Body example:
     ```json
     {
       "id": 1,
       "name": "Updated Product",
       "description": "Updated Description",
       "price": 149.99,
       "stock": 150
     }
     ```

   **DELETE /api/products/{id}**
   - Delete a product by ID

4. **Using curl** (alternative to Swagger):
   ```bash
   # Get all products
   curl http://localhost:5159/api/products

   # Create a product
   curl -X POST http://localhost:5159/api/products \
     -H "Content-Type: application/json" \
     -d '{"name":"Test Product","description":"Test","price":99.99,"stock":100}'

   # Get product by ID
   curl http://localhost:5159/api/products/1

   # Update product
   curl -X PUT http://localhost:5159/api/products/1 \
     -H "Content-Type: application/json" \
     -d '{"id":1,"name":"Updated","description":"Updated","price":149.99,"stock":150}'

   # Delete product
   curl -X DELETE http://localhost:5159/api/products/1
   ```

**Expected Output**:
- API starts and listens on configured port
- Swagger UI accessible
- All CRUD operations work correctly
- Proper HTTP status codes (200, 201, 404, etc.)

---

## 6. WeatherAPI ✅

**Status**: Running successfully  
**Type**: Interactive Console Application

**How to Test**:
```bash
cd WeatherAPI
dotnet run
```

**Test Steps**:

1. **Set API Key** (optional, but recommended):
   ```bash
   # Windows PowerShell
   $env:OPENWEATHER_API_KEY="your-api-key-here"
   
   # Windows CMD
   set OPENWEATHER_API_KEY=your-api-key-here
   
   # Linux/Mac
   export OPENWEATHER_API_KEY=your-api-key-here
   ```

2. **Get API Key** (if needed):
   - Visit: https://openweathermap.org/api
   - Sign up for a free account
   - Get your API key

3. **Run the Application**:
   ```bash
   dotnet run
   ```

4. **Test the Application**:
   - Enter a city name (e.g., "London", "New York", "Tokyo")
   - The application will fetch weather data
   - Enter 'exit' to quit

**Expected Output**:
- Prompt for city name
- Weather information for the entered city (if API key is set)
- Error message if API key is not set
- Option to exit

**Sample Output** (with API key):
```
Enter a city name to get weather information (or 'exit' to quit)
Enter city name: London
Weather in London: ...
Temperature: ...
```

**Sample Output** (without API key):
```
WARNING: Please set your OpenWeatherMap API key!
Enter a city name to get weather information (or 'exit' to quit)
```

---

## Running All Projects Sequentially

You can use the provided script to run all projects one by one:

**Bash Script**:
```bash
chmod +x run_projects_sequentially.sh
./run_projects_sequentially.sh
```

**Or run manually**:
```bash
# Project 1
cd LibraryManagementSystem/LibraryManagementSystem && dotnet run && cd ../..

# Project 2
cd FactorialCalculator/FactorialCalculator && dotnet run && cd ../..

# Project 3
cd ProducerConsumerPattern/ProducerConsumerPattern && dotnet run && cd ../..

# Project 4
cd EntityFrameworkCRUD && dotnet run && cd ..

# Project 5
cd ProductAPI && dotnet run && cd ..

# Project 6
cd WeatherAPI && dotnet run && cd ..
```

---

## Test Results Summary

✅ **All 6 projects running successfully**  
✅ **All projects targeting .NET 9.0**  
✅ **All 54 unit tests passing**  
✅ **Ready for manual testing**

---

## Notes

- **ProductAPI**: Runs as a web server - keep it running to test endpoints
- **WeatherAPI**: Requires API key for full functionality (optional)
- **EntityFrameworkCRUD**: Requires SQL Server or update connection string
- **Interactive Projects**: LibraryManagementSystem and WeatherAPI require user input
- **Auto-Run Projects**: FactorialCalculator and ProducerConsumerPattern run automatically

---

## Troubleshooting

1. **Framework Issues**: All projects now target .NET 9.0
2. **Port Conflicts**: If ProductAPI port is in use, update `launchSettings.json`
3. **Database Issues**: Update connection string in `appsettings.json` for EntityFrameworkCRUD
4. **API Key**: WeatherAPI works without API key but shows warning

---

## Next Steps

1. Test each project manually using the steps above
2. Verify all functionality works as expected
3. Check unit tests: `dotnet test`
4. Review code for any improvements

