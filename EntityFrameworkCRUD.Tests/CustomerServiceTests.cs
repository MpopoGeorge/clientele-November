using EntityFrameworkCRUD.Application.Interfaces;
using EntityFrameworkCRUD.Application.Services;
using EntityFrameworkCRUD.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace EntityFrameworkCRUD.Tests
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _mockRepository;
        private CustomerService _customerService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockRepository.Object);
        }

        [Test]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
                new Customer { CustomerId = 2, Name = "Jane Smith", Email = "jane@example.com", PhoneNumber = "0987654321" }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task GetCustomerByIdAsync_ExistingId_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CustomerId, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task GetCustomerByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateCustomerAsync_ValidCustomer_ReturnsCreatedCustomer()
        {
            // Arrange
            var newCustomer = new Customer { Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
            var createdCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerService.CreateCustomerAsync(newCustomer);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CustomerId, Is.EqualTo(1));
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public void CreateCustomerAsync_NullCustomer_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _customerService.CreateCustomerAsync(null!));
        }

        [Test]
        public void CreateCustomerAsync_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var customer = new Customer { Name = "", Email = "john@example.com", PhoneNumber = "1234567890" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _customerService.CreateCustomerAsync(customer));
        }

        [Test]
        public void CreateCustomerAsync_EmptyEmail_ThrowsArgumentException()
        {
            // Arrange
            var customer = new Customer { Name = "John Doe", Email = "", PhoneNumber = "1234567890" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _customerService.CreateCustomerAsync(customer));
        }

        [Test]
        public async Task UpdateCustomerAsync_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            var existingCustomer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
            var updatedCustomer = new Customer { CustomerId = 1, Name = "John Updated", Email = "john@example.com", PhoneNumber = "1234567890" };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCustomer);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).ReturnsAsync(true);

            // Act
            var result = await _customerService.UpdateCustomerAsync(updatedCustomer);

            // Assert
            Assert.That(result, Is.True);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public async Task UpdateCustomerAsync_NonExistingCustomer_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { CustomerId = 999, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.UpdateCustomerAsync(customer);

            // Assert
            Assert.That(result, Is.False);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Test]
        public async Task DeleteCustomerAsync_ExistingCustomer_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _customerService.DeleteCustomerAsync(1);

            // Assert
            Assert.That(result, Is.True);
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteCustomerAsync_NonExistingCustomer_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.DeleteCustomerAsync(999);

            // Assert
            Assert.That(result, Is.False);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task SearchCustomersByNameAsync_ValidName_ReturnsMatchingCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" }
            };

            _mockRepository.Setup(r => r.SearchByNameAsync("John")).ReturnsAsync(customers);

            // Act
            var result = await _customerService.SearchCustomersByNameAsync("John");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Contains.Substring("John"));
        }

        [Test]
        public async Task SearchCustomersByNameAsync_EmptyName_ReturnsEmptyList()
        {
            // Act
            var result = await _customerService.SearchCustomersByNameAsync("");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
            _mockRepository.Verify(r => r.SearchByNameAsync(It.IsAny<string>()), Times.Never);
        }
    }
}

