# ComandaX API - Architecture & Best Practices

## Overview

ComandaX API follows **Clean Architecture** principles with a clear separation of concerns across multiple layers. The application uses **CQRS** (Command Query Responsibility Segregation) pattern via MediatR and exposes a **GraphQL API** using HotChocolate.

## Architecture Layers

### 1. Domain Layer (`ComandaX.Domain`)

**Purpose**: Core business logic and entities

**Key Principles**:

- Contains pure domain entities with encapsulated business logic
- No dependencies on other layers
- Entities use private setters to enforce encapsulation
- Business rules are enforced through domain methods (e.g., `AddProduct()`, `StartPreparation()`)
- All entities inherit from `BaseEntity` which provides common properties (Id, CreatedAt, UpdatedAt)

**Example**:

```csharp
public sealed class Order : BaseEntity
{
    public void AddProduct(Guid productId, int quantity, decimal price)
    {
        var orderProduct = new OrderProduct(Id, productId, quantity, price);
        OrderProducts.Add(orderProduct);
        EntityUpdated();
    }
}
```

### 2. Application Layer (`ComandaX.Application`)

**Purpose**: Application business logic, use cases, and contracts

**Key Components**:

#### 2.1 CQRS Handlers

- **Commands**: Modify state (Create, Update, Delete operations)
- **Queries**: Read data without side effects
- Each handler has a single responsibility
- Handlers use constructor injection for dependencies

**Pattern**:

```
Handlers/
  ├── [Entity]/
  │   ├── Commands/
  │   │   └── [Action]/
  │   │       ├── [Action]Command.cs
  │   │       └── [Action]CommandHandler.cs
  │   └── Queries/
  │       └── [Action]/
  │           ├── [Action]Query.cs
  │           └── [Action]QueryHandler.cs
```

**Example**:

```csharp
public class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, Order>
{
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id)
            ?? throw new RecordNotFoundException(request.Id);
        return order;
    }
}
```

#### 2.2 DTOs (Data Transfer Objects)

- Immutable records for data transfer between layers
- Use record types for immutability and value semantics
- Optional properties use nullable types with default values
- DTOs are separate from domain entities to prevent tight coupling

**Pattern**:

```csharp
public sealed record OrderProductDto(
    Guid ProductId,
    int Quantity,
    decimal TotalPrice,
    ProductDto? Product = null
);
```

#### 2.3 Extension Methods

- Convert domain entities to DTOs
- Keep conversion logic centralized and reusable
- Named `AsDto()` for consistency

**Pattern**:

```csharp
public static class OrderExtension
{
    public static OrderDto AsDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.Code,
            order.CustomerTabId,
            [.. order.OrderProducts.Select(p => new OrderProductDto(p.ProductId, p.Quantity, p.TotalPrice))],
            order.Status
        );
    }
}
```

#### 2.4 Repository Interfaces

- Define contracts for data access
- Keep interfaces minimal and focused
- Include batch operations for DataLoader support (e.g., `GetByIdsAsync`)

**Pattern**:

```csharp
public interface IProductRepository
{
    Task<IList<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task<IList<Product>> GetByIdsAsync(IReadOnlyList<Guid> ids); // For DataLoader
}
```

### 3. Infrastructure Layer (`ComandaX.Infrastructure`)

**Purpose**: External concerns (database, external services)

**Key Components**:

#### 3.1 Repositories

- Implement repository interfaces from Application layer
- Use Entity Framework Core for data access
- Use primary constructor injection pattern
- Keep queries simple and focused

**Pattern**:

```csharp
public class ProductRepository(AppDbContext _context) : IProductRepository
{
    public async Task<IList<Product>> GetByIdsAsync(IReadOnlyList<Guid> ids)
    {
        return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }
}
```

#### 3.2 DbContext

- Configure entity relationships
- Use fluent API for complex configurations
- Keep migrations organized

#### 3.3 Dependency Injection

- Register all infrastructure services in `DependencyInjection.cs`
- Use scoped lifetime for repositories

### 4. WebAPI Layer (`ComandaX.WebAPI`)

**Purpose**: API presentation and GraphQL schema

**Key Components**:

#### 4.1 GraphQL Queries

- Extend the root Query type using `[ExtendObjectType("Query")]`
- Use MediatR to dispatch queries
- Apply `[UseProjection]` for IQueryable results to enable field selection
- Convert entities to DTOs before returning

**Pattern**:

```csharp
[ExtendObjectType("Query")]
public class OrderQuery
{
    [UseProjection]
    public async Task<IQueryable<OrderDto>> GetOrders([Service] IMediator mediator)
    {
        var orders = await mediator.Send(new GetOrdersQuery());
        return orders.Select(order => order.AsDto());
    }
}
```

#### 4.2 GraphQL Mutations

- Extend the root Mutation type using `[ExtendObjectType("Mutation")]`
- Use MediatR to dispatch commands
- Return appropriate DTOs or entities

#### 4.3 GraphQL Types

- Configure field types and resolvers
- Use `IsProjected()` for fields that should be included in database queries
- Use `Ignore()` for fields that shouldn't be directly queryable
- Configure resolvers for related entities

**Pattern**:

```csharp
public class OrderProductType : ObjectType<OrderProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderProductDto> descriptor)
    {
        descriptor.Field(o => o.ProductId).Type<NonNullType<IdType>>().IsProjected();
        descriptor.Field(o => o.Product)
            .ResolveWith<OrderProductResolvers>(r => r.GetProduct(default!, default!, default!))
            .Type<ProductType>();
    }
}
```

#### 4.4 DataLoaders

- Solve N+1 query problem in GraphQL
- Batch and cache database queries
- Implement `BatchDataLoader<TKey, TValue>`
- Repository must support batch operations (`GetByIdsAsync`)

**Pattern**:

```csharp
public class GetProductByIdDataLoader : BatchDataLoader<Guid, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IProductRepository productRepository)
        : base(batchScheduler, options)
    {
        _productRepository = productRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, Product>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetByIdsAsync(keys);
        return result.ToDictionary(p => p.Id);
    }
}
```

#### 4.5 Resolvers

- Handle field resolution for related entities
- Use DataLoaders to efficiently load related data
- Convert entities to DTOs
- Handle null cases gracefully

**Pattern**:

```csharp
public class OrderProductResolvers
{
    public async Task<ProductDto?> GetProduct(
        [Parent] OrderProductDto orderProduct,
        GetProductByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        var product = await dataLoader.LoadAsync(orderProduct.ProductId, cancellationToken);

        if (product == null)
            return null;

        return product.AsDto();
    }
}
```

## Design Patterns & Best Practices

### 1. CQRS (Command Query Responsibility Segregation)

- **Commands**: Modify state, return void or simple confirmation
- **Queries**: Read data, return DTOs or entities
- Separate handlers for each operation
- Use MediatR for request/response pattern

### 2. Repository Pattern

- Abstract data access logic
- Interface in Application layer, implementation in Infrastructure
- Keep methods focused and minimal
- Support batch operations for GraphQL DataLoaders
- Repositories do NOT call SaveChangesAsync - they only track changes
- SaveChanges is coordinated by the Unit of Work

### 3. Unit of Work Pattern

- Coordinates the work of multiple repositories
- Maintains a single database transaction across multiple operations
- Provides transaction management (Begin, Commit, Rollback)
- Ensures data consistency across multiple entity changes
- All command handlers use IUnitOfWork instead of individual repositories

**Pattern**:

```csharp
public class CreateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Access repositories through Unit of Work
        var product = await _unitOfWork.Products.GetByIdAsync(productId);

        var order = new Order();
        order.AddProduct(productId, quantity, product.Price);

        await _unitOfWork.Orders.AddAsync(order);

        // Save all changes in a single transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.AsDto();
    }
}
```

**For complex transactions**:

```csharp
public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
{
    await _unitOfWork.BeginTransactionAsync(cancellationToken);

    try
    {
        // Multiple operations
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.Tables.UpdateAsync(table);

        // Commit transaction
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return order.AsDto();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        throw;
    }
}
```

### 4. Dependency Injection

- Constructor injection throughout
- Use primary constructors in C# 12 for conciseness
- Register services in appropriate DI extension methods
- Use scoped lifetime for repositories, DbContext, and Unit of Work
- Command handlers inject IUnitOfWork, not individual repositories

### 5. GraphQL Best Practices

#### DataLoader Pattern

**When to use**: Whenever you need to load related entities in GraphQL
**Why**: Prevents N+1 query problem by batching database queries

**Implementation Steps**:

1. Add batch method to repository interface: `Task<IList<T>> GetByIdsAsync(IReadOnlyList<Guid> ids)`
2. Implement batch method in repository
3. Create DataLoader class extending `BatchDataLoader<Guid, Entity>`
4. Create Resolver class with method to resolve the field
5. Configure field in GraphQL Type to use resolver
6. Register DataLoader and Resolver in DI

#### Field Resolution

- Use `IsProjected()` for foreign key fields that should be included in queries
- Use `Ignore()` for navigation properties that shouldn't be directly queryable
- Use `ResolveWith<TResolver>()` to specify custom resolution logic

### 6. DTO Pattern

- Use immutable records
- Keep DTOs flat and simple
- Optional related entities should be nullable with default null
- Separate DTOs from domain entities

### 7. Extension Methods

- Centralize entity-to-DTO conversion
- Use consistent naming: `AsDto()`
- Keep conversions simple and focused

### 8. Error Handling

- Use custom exceptions (e.g., `RecordNotFoundException`)
- Throw exceptions in handlers, not in domain entities
- Let GraphQL middleware handle exception formatting
- Unit of Work automatically rolls back on exceptions

## Common Workflows

### Adding a New Entity with Related Data

1. **Domain Layer**: Create entity with business logic
2. **Application Layer**:
   - Create DTO with optional related entity property
   - Create repository interface with batch method
   - Create CQRS handlers
   - Create extension method for entity-to-DTO conversion
3. **Infrastructure Layer**:
   - Implement repository with batch method
   - Register repository in DI
4. **WebAPI Layer**:
   - Create GraphQL Query/Mutation
   - Create GraphQL Type with resolver configuration
   - Create DataLoader for related entity
   - Create Resolver for field resolution
   - Register DataLoader and Resolver in DI

### Example: Adding Product Information to Orders

This was implemented following these steps:

1. **Updated OrderProductDto** to include optional `ProductDto? Product = null`
2. **Added batch method** to `IProductRepository`: `GetByIdsAsync()`
3. **Implemented batch method** in `ProductRepository`
4. **Created DataLoader**: `GetProductByIdDataLoader`
5. **Created Resolver**: `OrderProductResolvers.GetProduct()`
6. **Updated GraphQL Type**: `OrderProductType` to use resolver
7. **Registered in DI**: Added DataLoader and Resolver to `ServiceCollectionExtensions`

## Technology Stack

- **.NET 8**: Latest LTS version
- **ASP.NET Core**: Web framework
- **Entity Framework Core 8**: ORM for database access
- **PostgreSQL**: Relational database
- **HotChocolate**: GraphQL server
- **MediatR**: CQRS implementation
- **xUnit & Moq**: Testing framework

## Project Structure

```
ComandaX-API/
├── src/
│   ├── ComandaX.Domain/           # Domain entities and enums
│   ├── ComandaX.Application/      # Business logic, DTOs, interfaces
│   │   ├── DTOs/
│   │   ├── Extensions/
│   │   ├── Handlers/
│   │   ├── Interfaces/
│   │   └── Exceptions/
│   ├── ComandaX.Infrastructure/   # Data access, repositories
│   │   └── Persistence/
│   │       └── Repository/
│   ├── ComandaX.WebAPI/          # GraphQL API
│   │   └── GraphQL/
│   │       ├── DataLoaders/
│   │       ├── Mutations/
│   │       ├── Queries/
│   │       ├── Resolvers/
│   │       └── Types/
│   └── ComandaX.Tests/           # Unit tests
└── ARCHITECTURE.md               # This file
```

## Key Takeaways

1. **Separation of Concerns**: Each layer has a specific responsibility
2. **CQRS**: Separate read and write operations
3. **Unit of Work**: Coordinate repository operations and manage transactions
4. **GraphQL Optimization**: Use DataLoaders to prevent N+1 queries
5. **Immutability**: Use records for DTOs, private setters for entities
6. **Dependency Injection**: Constructor injection throughout
7. **Clean Code**: Follow consistent naming and patterns
8. **Type Safety**: Leverage C# type system and nullable reference types
9. **Transaction Management**: Use Unit of Work for data consistency
