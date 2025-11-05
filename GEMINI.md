# ComandaX API Project Summary for AI

This document provides a detailed summary of the ComandaX API project, intended to be used as a knowledge base for an AI assistant.

## Project Overview

ComandaX is a backend API for a restaurant management system. It allows managing tables, customer tabs, products, and orders. The system is designed to be used in a restaurant environment where waiters can take orders for customers at different tables. The API is built using .NET and exposes its functionalities through a GraphQL interface.

The frontend that consumes this API is an Angular application built with standalone components.

## Architecture

The project follows the principles of **Clean Architecture**, separating concerns into different layers. This results in a loosely coupled, maintainable, and testable application. The layers are:

-   **Domain**: Contains the core business logic and entities.
-   **Application**: Orchestrates the data flow and contains the application-specific business rules.
-   **Infrastructure**: Handles external concerns like data persistence, external services, etc.
-   **WebAPI**: Exposes the application functionalities through a GraphQL API.

The project also implements the **Command Query Responsibility Segregation (CQRS)** pattern, using the MediatR library. This pattern separates read and write operations, allowing for more optimized and scalable data management.

## Project Structure

The solution is divided into several projects, each with a specific responsibility:

-   `ComandaX.Domain`: The core of the application, containing the domain entities and enums.
-   `ComandaX.Application`: Contains the application logic, including CQRS commands and queries, DTOs, and repository interfaces.
-   `ComandaX.Infrastructure`: Implements the interfaces defined in the Application layer, providing data persistence using Entity Framework Core.
-   `ComandaX.WebAPI`: The presentation layer, which exposes the application's features through a GraphQL API.
-   `ComandaX.Tests`: Contains unit tests for the application.

## Domain Layer

The Domain layer is the heart of the application and contains the following entities:

-   `BaseEntity`: An abstract class with common properties like `Id`, `CreatedAt`, `UpdatedAt`, and `DeletedAt`.
-   `CustomerTab`: Represents a customer's tab, which can be associated with a table and can have multiple orders.
-   `Order`: Represents an order made by a customer. It has a status (`Created`, `InPreparation`, `Closed`) and contains a list of products.
-   `Product`: Represents a product that can be ordered, with properties like `Name`, `Price`, and `Code`.
-   `Table`: Represents a table in the restaurant, with a `Code` and a `Status` (`Free`, `Busy`).
-   `OrderProduct`: A linking entity for the many-to-many relationship between `Order` and `Product`.

## Application Layer

The Application layer is responsible for the application's business logic and uses the CQRS pattern.

### Commands

-   **CustomerTabs**:
    -   `CreateCustomerTabCommand`: Creates a new customer tab.
-   **Orders**:
    -   `CreateOrderCommand`: Creates a new order for a customer tab.
    -   `AddProductsToOrderCommand`: Adds products to an existing order.
    -   `CloseOrderCommand`: Closes an order.
-   **Products**:
    -   `CreateProductCommand`: Creates a new product.
-   **Tables**:
    -   `CreateTableCommand`: Creates a new table.

### Queries

-   **CustomerTabs**:
    -   `GetCustomerTabByIdQuery`: Retrieves a customer tab by its ID.
    -   `GetCustomerTabsQuery`: Retrieves all customer tabs.
-   **Orders**:
    -   `GetOrderByIdQuery`: Retrieves an order by its ID.
    -   `GetOrdersQuery`: Retrieves all orders.
-   **Products**:
    -   `GetProductByIdQuery`: Retrieves a product by its ID.
    -   `GetProductsQuery`: Retrieves all products.
-   **Tables**:
    -   `GetTableByIdQuery`: Retrieves a table by its ID.
    -   `GetTablesQuery`: Retrieves all tables.

### DTOs

The Application layer also defines Data Transfer Objects (DTOs) to transfer data between layers:

-   `CustomerTabDto`
-   `OrderDto`
-   `OrderProductDto`
-   `ProductDto`
-   `TableDto`

### Repository Interfaces

This layer defines interfaces for the repositories, which are implemented in the Infrastructure layer:

-   `ICustomerTabRepository`
-   `IOrderRepository`
-   `IProductRepository`
-   `ITableRepository`

## Infrastructure Layer

The Infrastructure layer handles data persistence using Entity Framework Core with a PostgreSQL database. It provides implementations for the repository interfaces defined in the Application layer.

-   `AppDbContext`: The Entity Framework Core DbContext for the application.
-   **Repositories**: `CustomerTabRepository`, `OrderRepository`, `ProductRepository`, and `TableRepository`.
-   `DependencyInjection.cs`: A static class to configure the dependency injection for the infrastructure services.

## WebAPI Layer

The WebAPI layer exposes the application's functionalities through a GraphQL API using the HotChocolate library.

### GraphQL

-   **Queries**: `CustomerTabQuery`, `OrderQuery`, `ProductQuery`, `TableQuery`.
-   **Mutations**: `CustomerTabMutation`, `OrderMutation`, `ProductMutation`, `TableMutation`.
-   **Types**: `CustomerTabType`, `OrderType`, `ProductType`, `TableType`.
-   **DataLoaders**: `GetTableByIdDataLoader` to solve the N+1 problem in GraphQL.

The API is configured in `Program.cs` and uses a custom middleware (`ExceptionMiddleware`) for exception handling.

## Frontend

The frontend that consumes this API is an **Angular** application that uses **standalone components**. This modern Angular architecture promotes a more modular and simplified component structure.

## Technologies

-   **.NET 8**
-   **ASP.NET Core**
-   **Entity Framework Core 8**
-   **PostgreSQL**
-   **GraphQL (HotChocolate)**
-   **MediatR** (for CQRS)
-   **xUnit** and **Moq** (for testing)
-   **Docker**
