using EntityFrameworkCRUD.Application.Interfaces;
using EntityFrameworkCRUD.Domain.Entities;

namespace EntityFrameworkCRUD.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Customer>> SearchCustomersByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Customer>();

            return await _repository.SearchByNameAsync(name);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            ValidateCustomer(customer);
            return await _repository.CreateAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var existing = await _repository.GetByIdAsync(customer.CustomerId);
            if (existing == null)
                return false;

            ValidateCustomer(customer);
            return await _repository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                return false;

            return await _repository.DeleteAsync(id);
        }

        private void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Customer name is required", nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Customer email is required", nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
                throw new ArgumentException("Customer phone number is required", nameof(customer));
        }
    }
}

