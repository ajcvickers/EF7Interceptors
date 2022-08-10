# EF7Interceptors

Three EF Core interceptor examples from the .NET Blog, plus two more.

### EntityCaching

Caches instances of read-only entities so the same instance is always used across different `DbContext` instances

### InjectLogger

Injects an `ILogger` into entity instances when they are queried

### OptimisticConcurrencyInterception

Supprresses `DbUpdateConcurrencyException` for deletes

### QueryInterception

Automatically adds seconary ordering by primary keys

### SimpleMaterization

Sets a property on every entity when it is returned from the database
