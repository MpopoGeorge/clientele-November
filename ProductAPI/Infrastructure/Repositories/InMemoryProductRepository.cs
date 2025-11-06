using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private int _nextId = 1;

        public InMemoryProductRepository()
        {
            _products = new List<Product>
            {
                new Product { ProductID = _nextId++, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m },
                new Product { ProductID = _nextId++, Name = "Mouse", Description = "Wireless optical mouse", Price = 29.99m },
                new Product { ProductID = _nextId++, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m }
            };
        }

        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.ToList());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.ProductID == id);
            return Task.FromResult(product);
        }

        public Task<Product> CreateAsync(Product product)
        {
            product.ProductID = _nextId++;
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<bool> UpdateAsync(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (existing == null)
                return Task.FromResult(false);

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
                return Task.FromResult(false);

            _products.Remove(product);
            return Task.FromResult(true);
        }
    }
}

