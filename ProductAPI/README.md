# Product API

RESTful API for managing products.

## Endpoints

- GET `/api/products` - Get all products
- GET `/api/products/{id}` - Get product by ID
- POST `/api/products` - Create product
- PUT `/api/products/{id}` - Update product
- DELETE `/api/products/{id}` - Delete product

## Run

```bash
cd ProductAPI
dotnet run
```

Swagger UI: `http://localhost:5000/swagger`

## Notes

Uses in-memory storage. Data is lost on restart.
