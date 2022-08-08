// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using (var context = new CustomerContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.AddRange(
        new Customer
        {
            Name = "Alice",
            PhoneNumber = "+1 515 555 0123",
            City = "Ames"
        },
        new Customer
        {
            Name = "Mac",
            PhoneNumber = "+1 515 555 0124",
            City = "Ames"
        },
        new Customer { Name = "Toast" },
        new Customer { Name = "Baxter" });

    context.SaveChanges();
}

foreach (var customer in GetPageOfCustomers("City", 0))
{
    Console.WriteLine($"{customer.Name}");
}

List<Customer> GetPageOfCustomers(string sortProperty, int page)
{
    using var context = new CustomerContext();

    return context.Customers
        .OrderBy(e => EF.Property<object>(e, sortProperty))
        .Skip(page * 20).Take(20).ToList();
}

foreach (var customer in GetPageOfCustomers2("City", 0))
{
    Console.WriteLine($"{customer.Name}");
}

List<Customer> GetPageOfCustomers2(string sortProperty, int page)
{
    using var context = new CustomerContext();

    return context.Customers
        .OrderBy(e => EF.Property<object>(e, sortProperty))
        .ThenBy(e => e.Id)
        .Skip(page * 20).Take(20).ToList();
}

public class CustomerContext : DbContext
{
    private static readonly KeyOrderingExpressionInterceptor _keyOrderingExpressionInterceptor = new();

    public DbSet<Customer> Customers
        => Set<Customer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_keyOrderingExpressionInterceptor)
            .UseSqlite("Data Source = customers.db")
            .LogTo(Console.WriteLine, LogLevel.Information);
}

public class KeyOrderingExpressionInterceptor : IQueryExpressionInterceptor
{
    public Expression ProcessingQuery(Expression queryExpression, QueryExpressionEventData eventData)
        => new KeyOrderingExpressionVisitor().Visit(queryExpression);

    private class KeyOrderingExpressionVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo ThenByMethod
            = typeof(Queryable).GetMethods()
                .Single(m => m.Name == nameof(Queryable.ThenBy) && m.GetParameters().Length == 2);

        protected override Expression VisitMethodCall(MethodCallExpression? methodCallExpression)
        {
            var methodInfo = methodCallExpression!.Method;
            if (methodInfo.DeclaringType == typeof(Queryable)
                && methodInfo.Name == nameof(Queryable.OrderBy)
                && methodInfo.GetParameters().Length == 2)
            {
                var sourceType = methodCallExpression.Type.GetGenericArguments()[0];
                if (typeof(IHasIntKey).IsAssignableFrom(sourceType))
                {
                    var lambdaExpression = (LambdaExpression)((UnaryExpression)methodCallExpression.Arguments[1]).Operand;
                    var entityParameterExpression = lambdaExpression.Parameters[0];

                    return Expression.Call(
                        ThenByMethod.MakeGenericMethod(
                            sourceType,
                            typeof(int)),
                        methodCallExpression,
                        Expression.Lambda(
                            typeof(Func<,>).MakeGenericType(entityParameterExpression.Type, typeof(int)),
                            Expression.Property(entityParameterExpression, nameof(IHasIntKey.Id)),
                            entityParameterExpression));
                }
            }

            return base.VisitMethodCall(methodCallExpression);
        }
    }
}

public interface IHasIntKey
{
    int Id { get; }
}

public class Customer : IHasIntKey
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
}
