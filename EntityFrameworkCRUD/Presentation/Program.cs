using Bogus;
using EntityFrameworkCRUD.Application.DTOs;
using EntityFrameworkCRUD.Application.Interfaces;
using EntityFrameworkCRUD.Application.Services;
using EntityFrameworkCRUD.Domain.Entities;
using EntityFrameworkCRUD.Infrastructure.Data;
using EntityFrameworkCRUD.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Text.RegularExpressions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/app_log.txt", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    var seedSettings = configuration.GetSection("SeedSettings").Get<SeedSettings>() ?? new SeedSettings();

    var services = new ServiceCollection();
    ConfigureServices(services, configuration);

    var serviceProvider = services.BuildServiceProvider();

    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("CustomerApp");
    logger.LogInformation("Application started");

    var dbContext = serviceProvider.GetRequiredService<CustomerDbContext>();
    dbContext.Database.EnsureCreated();
    SeedData(dbContext, logger, seedSettings);

    var customerService = serviceProvider.GetRequiredService<ICustomerService>();

    await RunApplicationAsync(customerService, logger);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    Console.WriteLine("A fatal error occurred. Please check logs.");
}
finally
{
    Log.CloseAndFlush();
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddLogging(builder =>
    {
        builder.AddSerilog();
    });

    services.AddDbContext<CustomerDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ICustomerRepository, CustomerRepository>();

    services.AddScoped<ICustomerService, CustomerService>();
}

static async Task RunApplicationAsync(ICustomerService customerService, Microsoft.Extensions.Logging.ILogger logger)
{
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

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await SafeExecuteAsync(async () =>
                {
                    var customers = await customerService.GetAllCustomersAsync();
                    DisplayCustomers(customers);
                }, logger);
                break;

            case "2":
                Console.Write("Enter customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    await SafeExecuteAsync(async () =>
                    {
                        var customer = await customerService.GetCustomerByIdAsync(id);
                        if (customer != null)
                            Console.WriteLine(customer);
                        else
                            Console.WriteLine("Customer not found.");
                    }, logger);
                }
                else
                {
                    Console.WriteLine("Invalid ID.");
                }
                break;

            case "3":
                Console.Write("Enter name to search: ");
                string? search = Console.ReadLine();
                await SafeExecuteAsync(async () =>
                {
                    var customers = await customerService.SearchCustomersByNameAsync(search ?? "");
                    DisplayCustomers(customers);
                }, logger);
                break;

            case "4":
                await SafeExecuteAsync(async () =>
                {
                    var newCustomer = CollectCustomerInfo();
                    var created = await customerService.CreateCustomerAsync(newCustomer);
                    Console.WriteLine($"Customer created with ID: {created.CustomerId}");
                    logger.LogInformation("Customer created: {@Customer}", created);
                }, logger);
                break;

            case "5":
                Console.Write("Enter ID of customer to update: ");
                if (int.TryParse(Console.ReadLine(), out int updateId))
                {
                    await SafeExecuteAsync(async () =>
                    {
                        var existing = await customerService.GetCustomerByIdAsync(updateId);
                        if (existing != null)
                        {
                            Console.WriteLine("Current: " + existing);
                            Console.WriteLine("Enter new details:");
                            var updated = CollectCustomerInfo();
                            updated.CustomerId = updateId;
                            if (await customerService.UpdateCustomerAsync(updated))
                            {
                                Console.WriteLine("Updated.");
                                logger.LogInformation("Customer updated: {@Customer}", updated);
                            }
                            else
                            {
                                Console.WriteLine("Update failed.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Customer not found.");
                        }
                    }, logger);
                }
                else
                {
                    Console.WriteLine("Invalid ID.");
                }
                break;

            case "6":
                Console.Write("Enter ID of customer to delete: ");
                if (int.TryParse(Console.ReadLine(), out int deleteId))
                {
                    await SafeExecuteAsync(async () =>
                    {
                        if (await customerService.DeleteCustomerAsync(deleteId))
                        {
                            Console.WriteLine("Deleted.");
                            logger.LogInformation("Customer deleted with ID: {Id}", deleteId);
                        }
                        else
                        {
                            Console.WriteLine("Delete failed or customer not found.");
                        }
                    }, logger);
                }
                else
                {
                    Console.WriteLine("Invalid ID.");
                }
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

static async Task SafeExecuteAsync(Func<Task> action, Microsoft.Extensions.Logging.ILogger logger)
{
    try
    {
        await action();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during operation.");
        Console.WriteLine("An error occurred. Check logs for details.");
    }
}

static void SeedData(CustomerDbContext context, Microsoft.Extensions.Logging.ILogger logger, SeedSettings settings)
{
    if (context.Customers.Any())
    {
        logger.LogInformation("Clearing existing customer data...");
        context.Customers.RemoveRange(context.Customers);
        context.SaveChanges();
        logger.LogInformation("Existing data cleared.");
    }
    
    logger.LogInformation("Seeding {Count} fake customers with South African data...", settings.GenerateCount);

    var firstNames = new[] { "Thabo", "Sipho", "Lungile", "Nomsa", "Bongani", "Zanele", "Mandla", "Ntombi", 
        "Sibusiso", "Nolwazi", "Mpho", "Nthabiseng", "Kagiso", "Lerato", "Tshepo", "Puleng", 
        "Mthunzi", "Nokuthula", "Sizwe", "Nompumelelo", "Buhle", "Nkosinathi", "Naledi", "Sanele",
        "Thandeka", "Mzwandile", "Noluthando", "Sifiso", "Nqobile", "Lindiwe", "Mfundo", "Nombuso",
        "Siphesihle", "Ntando", "Lwandle", "Nokwanda", "Mthokozisi", "Nolubabalo", "Sibongile", "Ntokozo" };
    
    var lastNames = new[] { "Mthembu", "Dlamini", "Ndlovu", "Khumalo", "Mkhize", "Ntuli", "Mabena", "Zulu",
        "Molefe", "Nkomo", "Mabaso", "Moleko", "Nkosi", "Mahlangu", "Maseko", "Nxumalo",
        "Mabena", "Mthembu", "Dlamini", "Ndlovu", "Khumalo", "Mkhize", "Ntuli", "Mabena",
        "Zulu", "Molefe", "Nkomo", "Mabaso", "Moleko", "Nkosi", "Mahlangu", "Maseko",
        "Nxumalo", "Mabena", "Mthembu", "Dlamini", "Ndlovu", "Khumalo", "Mkhize", "Ntuli" };

    var phonePrefixes = new[] { "082", "083", "084", "071", "072", "073", "074", "079", "011", "021", "031", "041", "012", "016" };

    var faker = new Faker<Customer>("en_ZA")
        .RuleFor(c => c.Name, f => 
        {
            var firstName = f.PickRandom(firstNames);
            var lastName = f.PickRandom(lastNames);
            return $"{firstName} {lastName}";
        })
        .RuleFor(c => c.Email, f => 
        {
            var name = f.Name.FirstName().ToLower().Replace(" ", "");
            var domains = new[] { "gmail.co.za", "yahoo.co.za", "outlook.co.za", "webmail.co.za", "mweb.co.za", "telkomsa.net", "vodamail.co.za" };
            return $"{name}@{f.PickRandom(domains)}";
        })
        .RuleFor(c => c.PhoneNumber, f => 
        {
            var prefix = f.PickRandom(phonePrefixes);
            var number = f.Random.Int(1000000, 9999999);
            return $"{prefix} {number:### ####}";
        });

    var fakeCustomers = faker.Generate(settings.GenerateCount);

    context.Customers.AddRange(fakeCustomers);
    context.SaveChanges();
    logger.LogInformation("Seeded {Count} customers with South African data.", fakeCustomers.Count);
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
    string? input;
    do
    {
        Console.Write(message);
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            Console.WriteLine("Field is required.");
    } while (string.IsNullOrWhiteSpace(input));
    return input;
}

static string PromptValidated(string message, Func<string, bool> validate, string errorMsg)
{
    string? input;
    do
    {
        Console.Write(message);
        input = Console.ReadLine();
        if (input == null || !validate(input))
        {
            Console.WriteLine(errorMsg);
            input = null;
        }
    } while (string.IsNullOrWhiteSpace(input));
    return input ?? "";
}

static bool IsValidEmail(string email) =>
    Regex.IsMatch(email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

static bool IsValidPhoneNumber(string phone) =>
    Regex.IsMatch(phone ?? "", @"^\d{7,}$");

