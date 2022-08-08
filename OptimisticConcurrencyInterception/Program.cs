// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using (var context = new CustomerContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.AddRange(
        new Customer { Name = "Bill" },
        new Customer { Name = "Bob" });

    context.SaveChanges();
}

using (var context1 = new CustomerContext())
{
    var customer1 = context1.Customers.Single(e => e.Name == "Bill");

    using (var context2 = new CustomerContext())
    {
        var customer2 = context1.Customers.Single(e => e.Name == "Bill");
        context2.Entry(customer2).State = EntityState.Deleted;
        context2.SaveChanges();
    }

    context1.Entry(customer1).State = EntityState.Deleted;
    context1.SaveChanges();
}

public class CustomerContext : DbContext
{
    private static readonly SuppressDeleteConcurrencyInterceptor _concurrencyInterceptor = new();

    public DbSet<Customer> Customers
        => Set<Customer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_concurrencyInterceptor)
            .UseSqlite("Data Source = customers.db")
            .LogTo(Console.WriteLine, LogLevel.Information);
}

public class SuppressDeleteConcurrencyInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult ThrowingConcurrencyException(
        ConcurrencyExceptionEventData eventData,
        InterceptionResult result)
    {
        if (eventData.Entries.All(e => e.State == EntityState.Deleted))
        {
            Console.WriteLine("Suppressing Concurrency violation for command:");
            Console.WriteLine(((RelationalConcurrencyExceptionEventData)eventData).Command.CommandText);

            return InterceptionResult.Suppress();
        }

        return result;
    }

    public ValueTask<InterceptionResult> ThrowingConcurrencyExceptionAsync(
        ConcurrencyExceptionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
        => new(ThrowingConcurrencyException(eventData, result));
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
