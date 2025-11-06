using Microsoft.EntityFrameworkCore;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}