// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using (var context = new CustomerContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.AddRange(
        new Customer { Name = "Alice", PhoneNumber = "+1 515 555 0123" },
        new Customer { Name = "Mac", PhoneNumber = "+1 515 555 0124" });

    context.SaveChanges();
}

using (var context = new CustomerContext())
{
    var customer = context.Customers.Single(e => e.Name == "Alice");
    Console.WriteLine($"Customer '{customer.Name}' was retrieved at '{customer.Retrieved.ToLocalTime()}'");
}

public class CustomerContext : DbContext
{
    private static readonly SetRetrievedInterceptor _setRetrievedInterceptor = new();

    public DbSet<Customer> Customers
        => Set<Customer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_setRetrievedInterceptor)
            .UseSqlite("Data Source = customers.db");
}

public class SetRetrievedInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is IHasRetrieved hasRetrieved)
        {
            hasRetrieved.Retrieved = DateTime.UtcNow;
        }

        return instance;
    }
}

public interface IHasRetrieved
{
    DateTime Retrieved { get; set; }
}

public class Customer : IHasRetrieved
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    [NotMapped]
    public DateTime Retrieved { get; set; }
}
