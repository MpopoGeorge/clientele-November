# Product API

A RESTful API built with ASP.NET Core that manages a collection of Products. This project follows Clean Architecture principles.

## Features

- **CRUD Operations** for Products:
  - Create new products
  - Read all products or by ID
  - Update existing products
  - Delete products

- **Product Properties**:
  - ProductID (auto-generated)
  - Name (required)
  - Description (required)
  - Price (required, must be non-negative)

## Architecture

This project follows Clean Architecture with the following layers:

- **Domain**: Product entity
- **Application**: Interfaces (IProductRepository, IProductService) and Services (ProductService)
- **Infrastructure**: In-memory repository implementation
- **Presentation**: REST API Controllers

## Prerequisites

- .NET 8.0 SDK or higher
- Visual Studio, VS Code, or any IDE of your choice

## Running the API

1. Navigate to the ProductAPI directory:
```bash
cd ProductAPI
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. The API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### GET /api/products
Get all products

**Response**: List of all products

### GET /api/products/{id}
Get a product by ID

**Response**: Product object

### POST /api/products
Create a new product

**Request Body**:
```json
{
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99
}
```

**Response**: Created product with generated ProductID

### PUT /api/products/{id}
Update an existing product

**Request Body**:
```json
{
  "productID": 1,
  "name": "Updated Name",
  "description": "Updated Description",
  "price": 149.99
}
```

**Response**: 204 No Content on success

### DELETE /api/products/{id}
Delete a product

**Response**: 204 No Content on success

## Example Usage

### Using curl:

```bash
# Get all products
curl https://localhost:5001/api/products

# Get product by ID
curl https://localhost:5001/api/products/1

# Create a new product
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Laptop","description":"High-performance laptop","price":999.99}'

# Update a product
curl -X PUT https://localhost:5001/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"productID":1,"name":"Updated Laptop","description":"Updated description","price":1099.99}'

# Delete a product
curl -X DELETE https://localhost:5001/api/products/1
```

## Notes

- The current implementation uses an in-memory repository, so data is lost when the application restarts
- For production use, consider implementing a database-backed repository (Entity Framework Core, etc.)
- All endpoints include proper error handling and validation

