// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

var serviceProvider = new ServiceCollection()
    .AddDbContext<CustomerContext>(
        b => b.UseLoggerFactory(loggerFactory)
            .UseSqlite("Data Source = customers.db"))
    .BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.AddRange(
        new Customer { Name = "Alice", PhoneNumber = "+1 515 555 0123" },
        new Customer { Name = "Mac", PhoneNumber = "+1 515 555 0124" });

    context.SaveChanges();
}

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerContext>();

    var customer = context.Customers.Single(e => e.Name == "Alice");
    customer.PhoneNumber = "+1 515 555 0125";
}

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers
        => Set<Customer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new LoggerInjectionInterceptor());
}

public class LoggerInjectionInterceptor : IMaterializationInterceptor
{
    private ILogger? _logger;

    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is IHasLogger hasLogger)
        {
            _logger ??= materializationData.Context.GetService<ILoggerFactory>().CreateLogger("CustomersLogger");
            hasLogger.Logger = _logger;
        }

        return instance;
    }
}

public interface IHasLogger
{
    ILogger? Logger { get; set; }
}

public class Customer : IHasLogger
{
    private string? _phoneNumber;

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            Logger?.LogInformation(1, $"Updating phone number for '{Name}' from '{_phoneNumber}' to '{value}'.");

            _phoneNumber = value;
        }
    }

    [NotMapped]
    public ILogger? Logger { get; set; }
}
