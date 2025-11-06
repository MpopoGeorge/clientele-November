using System.Collections.Generic;
using System.Linq;

public class CustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public List<Customer> GetAll() => _context.Customers.ToList();

    public Customer GetById(int id) => _context.Customers.Find(id);

    public List<Customer> SearchByName(string name) =>
        _context.Customers.Where(c => c.Name.Contains(name)).ToList();

    public Customer Create(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return customer;
    }

    public bool Update(Customer customer)
    {
        _context.Customers.Update(customer);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(int id)
    {
        var customer = _context.Customers.Find(id);
        if (customer == null) return false;

        _context.Customers.Remove(customer);
        return _context.SaveChanges() > 0;
    }
}