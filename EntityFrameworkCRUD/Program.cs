using Bogus;
using EntityFrameworkCRUD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text.RegularExpressions;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var seedSettings = configuration.GetSection("SeedSettings").Get<SeedSettings>() ?? new SeedSettings();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/app_log.txt", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});

var logger = loggerFactory.CreateLogger("CustomerApp");
logger.LogInformation("Application started");

try
{
    var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    using (var context = new CustomerDbContext(optionsBuilder.Options))
    {
        context.Database.EnsureCreated();
        SeedData(context, logger, seedSettings);

        var repository = new CustomerRepository(context);
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== Customer Management System =====");
            Console.WriteLine("1. View all customers");
            Console.WriteLine("2. Find customer by ID");
            Console.WriteLine("3. Search customers by name");
            Console.WriteLine("4. Add new customer");
            Console.WriteLine("5. Update customer");
            Console.WriteLine("6. Delete customer");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice (1-7): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SafeExecute(() => DisplayCustomers(repository.GetAll()), logger);
                    break;
                case "2":
                    Console.Write("Enter customer ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                        SafeExecute(() => Console.WriteLine(repository.GetById(id)), logger);
                    else Console.WriteLine("Invalid ID.");
                    break;
                case "3":
                    Console.Write("Enter name to search: ");
                    string search = Console.ReadLine();
                    SafeExecute(() => DisplayCustomers(repository.SearchByName(search)), logger);
                    break;
                case "4":
                    SafeExecute(() =>
                    {
                        var newCustomer = CollectCustomerInfo();
                        var created = repository.Create(newCustomer);
                        Console.WriteLine($"Customer created with ID: {created.CustomerId}");
                        logger.LogInformation("Customer created: {@Customer}", created);
                    }, logger);
                    break;
                case "5":
                    Console.Write("Enter ID of customer to update: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        SafeExecute(() =>
                        {
                            var existing = repository.GetById(updateId);
                            if (existing != null)
                            {
                                Console.WriteLine("Current: " + existing);
                                Console.WriteLine("Enter new details:");
                                var updated = CollectCustomerInfo();
                                updated.CustomerId = updateId;
                                if (repository.Update(updated))
                                {
                                    Console.WriteLine("Updated.");
                                    logger.LogInformation("Customer updated: {@Customer}", updated);
                                }
                                else Console.WriteLine("Update failed.");
                            }
                            else Console.WriteLine("Customer not found.");
                        }, logger);
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;
                case "6":
                    Console.Write("Enter ID of customer to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        SafeExecute(() =>
                        {
                            if (repository.Delete(deleteId))
                            {
                                Console.WriteLine("Deleted.");
                                logger.LogInformation("Customer deleted with ID: {Id}", deleteId);
                            }
                            else Console.WriteLine("Delete failed.");
                        }, logger);
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;
                case "7":
                    running = false;
                    Console.WriteLine("Exiting...");
                    logger.LogInformation("Application exiting.");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Unhandled exception occurred.");
    Console.WriteLine("A fatal error occurred. Please check logs.");
}
finally
{
    Log.CloseAndFlush();
}

static void SafeExecute(Action action, Microsoft.Extensions.Logging.ILogger logger)
{
    try { action(); }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during operation.");
        Console.WriteLine("An error occurred. Check logs for details.");
    }
}

static void SeedData(CustomerDbContext context, Microsoft.Extensions.Logging.ILogger logger, SeedSettings settings)
{
    if (!context.Customers.Any())
    {
        logger.LogInformation("Seeding {Count} fake customers...", settings.GenerateCount);

        var faker = new Faker<Customer>()
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("0#########"));

        var fakeCustomers = faker.Generate(settings.GenerateCount);

        context.Customers.AddRange(fakeCustomers);
        context.SaveChanges();
        logger.LogInformation("Seeded {Count} customers.", fakeCustomers.Count);
    }
    else logger.LogInformation("Database already contains customers. Skipping seed.");
}

static void DisplayCustomers(List<Customer> customers)
{
    if (customers == null || customers.Count == 0)
    {
        Console.WriteLine("No customers found.");
        return;
    }

    Console.WriteLine("\nCustomers:");
    foreach (var c in customers)
        Console.WriteLine(c);
}

static Customer CollectCustomerInfo()
{
    var customer = new Customer
    {
        Name = PromptNonEmpty("Enter name: "),
        Email = PromptValidated("Enter email: ", IsValidEmail, "Invalid email."),
        PhoneNumber = PromptValidated("Enter phone number: ", IsValidPhoneNumber, "Invalid phone number.")
    };
    return customer;
}

static string PromptNonEmpty(string message)
{
    string input;
    do
    {
        Console.Write(message);
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) Console.WriteLine("Field is required.");
    } while (string.IsNullOrWhiteSpace(input));
    return input;
}

static string PromptValidated(string message, Func<string, bool> validate, string errorMsg)
{
    string input;
    do
    {
        Console.Write(message);
        input = Console.ReadLine();
        if (!validate(input))
        {
            Console.WriteLine(errorMsg);
            input = null;
        }
    } while (string.IsNullOrWhiteSpace(input));
    return input;
}

static bool IsValidEmail(string email) =>
    Regex.IsMatch(email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

static bool IsValidPhoneNumber(string phone) =>
    Regex.IsMatch(phone ?? "", @"^\d{7,}$");
