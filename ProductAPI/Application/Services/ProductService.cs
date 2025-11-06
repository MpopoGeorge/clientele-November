using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            ValidateProduct(product);
            return await _repository.CreateAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existing = await _repository.GetByIdAsync(product.ProductID);
            if (existing == null)
                return false;

            ValidateProduct(product);
            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return false;

            return await _repository.DeleteAsync(id);
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required", nameof(product));

            if (string.IsNullOrWhiteSpace(product.Description))
                throw new ArgumentException("Product description is required", nameof(product));

            if (product.Price < 0)
                throw new ArgumentException("Product price cannot be negative", nameof(product));
        }
    }
}

