using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new CustomerDbContext(optionsBuilder.Options);
    }
}
