using Moq;
using NUnit.Framework;
using ProductAPI.Application.Interfaces;
using ProductAPI.Application.Services;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockRepository;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockRepository.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { ProductID = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m },
                new Product { ProductID = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public async Task GetProductByIdAsync_ExistingId_ReturnsProduct()
        {
            // Arrange
            var product = new Product { ProductID = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductID, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public async Task GetProductByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetProductByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateProductAsync_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product { Name = "Laptop", Description = "High-performance laptop", Price = 999.99m };
            var createdProduct = new Product { ProductID = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(newProduct);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductID, Is.EqualTo(1));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public void CreateProductAsync_NullProduct_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _productService.CreateProductAsync(null!));
        }

        [Test]
        public void CreateProductAsync_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Name = "", Description = "Description", Price = 99.99m };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _productService.CreateProductAsync(product));
        }

        [Test]
        public void CreateProductAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Name = "Product", Description = "Description", Price = -10m };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _productService.CreateProductAsync(product));
        }

        [Test]
        public async Task UpdateProductAsync_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var existingProduct = new Product { ProductID = 1, Name = "Laptop", Description = "Description", Price = 999.99m };
            var updatedProduct = new Product { ProductID = 1, Name = "Updated Laptop", Description = "Updated Description", Price = 1099.99m };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var result = await _productService.UpdateProductAsync(updatedProduct);

            // Assert
            Assert.That(result, Is.True);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_NonExistingProduct_ReturnsFalse()
        {
            // Arrange
            var product = new Product { ProductID = 999, Name = "Product", Description = "Description", Price = 99.99m };
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.UpdateProductAsync(product);

            // Assert
            Assert.That(result, Is.False);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductAsync_ExistingProduct_ReturnsTrue()
        {
            // Arrange
            var product = new Product { ProductID = 1, Name = "Laptop", Description = "Description", Price = 999.99m };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProductAsync(1);

            // Assert
            Assert.That(result, Is.True);
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_NonExistingProduct_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.DeleteProductAsync(999);

            // Assert
            Assert.That(result, Is.False);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}

