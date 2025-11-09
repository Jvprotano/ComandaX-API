# Validation and Error Handling in GraphQL

## Overview

This document explains how validation errors and application exceptions are handled in the ComandaX GraphQL API.

## Problem

Previously, `FluentValidation` exceptions thrown by the `ValidationBehavior` in MediatR handlers were **not being caught by the ASP.NET Core `ExceptionMiddleware`**. This is because:

1. **GraphQL has its own exception handling pipeline** that intercepts exceptions before they reach ASP.NET Core middleware
2. The `ExceptionMiddleware` only handles HTTP requests, but GraphQL requests are processed within the GraphQL execution engine
3. Exceptions thrown in MediatR handlers are caught by GraphQL's error handling system, not the HTTP middleware

## Solution

We implemented a **GraphQL Error Filter** (`ValidationErrorFilter`) that intercepts exceptions within the GraphQL execution pipeline and converts them to properly formatted GraphQL errors.

### Architecture

```
┌─────────────────────────────────────────────────────────┐
│ GraphQL Request                                         │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│ GraphQL Execution Engine                                │
│ - Resolves queries/mutations                            │
│ - Calls MediatR handlers                                │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│ MediatR Pipeline                                        │
│ - ValidationBehavior (throws ValidationException)       │
│ - Handler execution                                     │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│ GraphQL Error Filter (ValidationErrorFilter)            │ ← NEW
│ - Catches ValidationException                           │
│ - Catches RecordNotFoundException                       │
│ - Catches UserNotAuthorizedException                    │
│ - Catches OrderWithoutProductsException                 │
│ - Formats errors for GraphQL response                   │
└─────────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────────┐
│ GraphQL Response (with formatted errors)                │
└─────────────────────────────────────────────────────────┘
```

## Implementation

### 1. ValidationErrorFilter

Located at: `src/ComandaX.WebAPI/GraphQL/Filters/ValidationErrorFilter.cs`

This filter implements `IErrorFilter` from HotChocolate and handles:

- **FluentValidation.ValidationException** → Returns `VALIDATION_ERROR` with field-level errors
- **RecordNotFoundException** → Returns `NOT_FOUND` error
- **UserNotAuthorizedException** → Returns `UNAUTHORIZED` error
- **OrderWithoutProductsException** → Returns `BUSINESS_RULE_VIOLATION` error

### 2. Registration

The filter is registered in `ServiceCollectionExtensions.cs`:

```csharp
services
    .AddGraphQLServer()
    // ... other configurations
    .AddErrorFilter<ValidationErrorFilter>()
    // ... other configurations
```

## Error Response Format

### Validation Errors

When a validation error occurs, the GraphQL response will look like:

```json
{
  "errors": [
    {
      "message": "Validation failed",
      "extensions": {
        "code": "VALIDATION_ERROR",
        "validationErrors": [
          {
            "field": "Name",
            "message": "Product name is required"
          },
          {
            "field": "Price",
            "message": "Price must be greater than zero"
          }
        ]
      }
    }
  ],
  "data": {
    "createProduct": null
  }
}
```

### Not Found Errors

```json
{
  "errors": [
    {
      "message": "Product with Id 123e4567-e89b-12d3-a456-426614174000 not found.",
      "extensions": {
        "code": "NOT_FOUND"
      }
    }
  ],
  "data": {
    "product": null
  }
}
```

### Unauthorized Errors

```json
{
  "errors": [
    {
      "message": "User with email user@example.com is not authorized.",
      "extensions": {
        "code": "UNAUTHORIZED"
      }
    }
  ],
  "data": {
    "authenticateWithGoogle": null
  }
}
```

### Business Rule Violations

```json
{
  "errors": [
    {
      "message": "Order must have at least one product",
      "extensions": {
        "code": "BUSINESS_RULE_VIOLATION"
      }
    }
  ],
  "data": {
    "createOrder": null
  }
}
```

## Testing Validation

### Example: Create Product with Invalid Data

**GraphQL Mutation:**
```graphql
mutation {
  createProduct(
    name: ""
    price: -10
  ) {
    id
    name
    price
  }
}
```

**Expected Response:**
```json
{
  "errors": [
    {
      "message": "Validation failed",
      "extensions": {
        "code": "VALIDATION_ERROR",
        "validationErrors": [
          {
            "field": "Name",
            "message": "Product name is required"
          },
          {
            "field": "Price",
            "message": "Price must be greater than zero"
          }
        ]
      }
    }
  ],
  "data": {
    "createProduct": null
  }
}
```

## Existing Validators

The application has the following FluentValidation validators:

1. **CreateProductCommandValidator** - Validates product creation
2. **UpdateProductCommandValidator** - Validates product updates
3. **DeleteProductCommandValidator** - Validates product deletion
4. **CreateOrderCommandValidator** - Validates order creation
5. **AddProductsToOrderCommandValidator** - Validates adding products to orders
6. **CreateTableCommandValidator** - Validates table creation
7. **CreateProductCategoryCommandValidator** - Validates category creation
8. **UpdateProductCategoryCommandValidator** - Validates category updates
9. **DeleteProductCategoryCommandValidator** - Validates category deletion

All of these validators are now properly handled by the GraphQL error filter.

## Benefits

1. ✅ **Consistent Error Format** - All errors follow GraphQL error specification
2. ✅ **Client-Friendly** - Errors include error codes and structured data
3. ✅ **Type-Safe** - Validation errors include field names for easy mapping
4. ✅ **Centralized** - All exception handling in one place
5. ✅ **Maintainable** - Easy to add new exception types

## Notes

- The ASP.NET Core `ExceptionMiddleware` is still in place for non-GraphQL endpoints (like `/` health check)
- GraphQL errors are handled by the `ValidationErrorFilter` within the GraphQL pipeline
- This is the correct pattern for HotChocolate GraphQL servers

