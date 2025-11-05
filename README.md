# ComandaX-API

ComandaX is a backend API for a restaurant management system. It allows managing tables, customer tabs, products, and orders. The system is designed to be used in a restaurant environment where waiters can take orders for customers at different tables. The API is built using .NET and exposes its functionalities through a GraphQL interface.

The frontend that consumes this API is an Angular application built with standalone components.

## Technologies

-   **.NET 8**
-   **ASP.NET Core**
-   **Entity Framework Core 8**
-   **PostgreSQL**
-   **GraphQL (HotChocolate)**
-   **MediatR** (for CQRS)
-   **xUnit** and **Moq** (for testing)
-   **Docker**

## Project Structure

The solution is divided into several projects, each with a specific responsibility:

-   `ComandaX.Domain`: The core of the application, containing the domain entities and enums.
-   `ComandaX.Application`: Contains the application logic, including CQRS commands and queries, DTOs, and repository interfaces.
-   `ComandaX.Infrastructure`: Implements the interfaces defined in the Application layer, providing data persistence using Entity Framework Core.
-   `ComandaX.WebAPI`: The presentation layer, which exposes the application's features through a GraphQL API.
-   `ComandaX.Tests`: Contains unit tests for the application.

## How to run

### Using .NET CLI

1.  Clone the repository
2.  Navigate to `src/ComandaX.WebAPI`
3.  Run `dotnet run`

### Using Docker

1.  Clone the repository
2.  Run `docker build -t comandax-api .`
3.  Run `docker run -p 5012:8080 comandax-api`

