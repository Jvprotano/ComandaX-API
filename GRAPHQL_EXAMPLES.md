# GraphQL Query Examples

This document contains example GraphQL queries to test the ComandaX API, specifically demonstrating how product information is now included when retrieving orders and customer tabs.

## Testing Product Information in Orders

### Query 1: Get Orders with Product Details

This query retrieves all orders and includes full product information for each order item:

```graphql
query GetOrdersWithProducts {
  orders {
    id
    code
    status
    products {
      productId
      quantity
      totalPrice
      product {
        id
        name
        price
        code
        needPreparation
      }
    }
  }
}
```

**What this demonstrates:**
- Orders now include product details through the `product` field in `OrderProductDto`
- The DataLoader pattern efficiently batches product queries to prevent N+1 problems
- Clients can request as much or as little product information as needed

### Query 2: Get Single Order with Product Details

```graphql
query GetOrderById {
  orderById(id: "YOUR_ORDER_ID_HERE") {
    id
    code
    status
    customerTabId
    products {
      productId
      quantity
      totalPrice
      product {
        id
        name
        price
        code
        needPreparation
        productCategoryId
      }
    }
  }
}
```

### Query 3: Get Orders with Selective Product Fields

GraphQL allows clients to request only the fields they need:

```graphql
query GetOrdersWithMinimalProductInfo {
  orders {
    id
    code
    products {
      quantity
      totalPrice
      product {
        name
        price
      }
    }
  }
}
```

## Testing Product Information in Customer Tabs

### Query 4: Get Customer Tabs with Orders and Products

This demonstrates the full relationship chain: CustomerTab → Orders → Products

```graphql
query GetCustomerTabsWithOrdersAndProducts {
  customerTabs {
    id
    name
    table {
      id
      number
      status
    }
  }
}
```

Note: To get orders for a customer tab, you would need to query orders filtered by customerTabId:

```graphql
query GetOrdersForCustomerTab {
  orders {
    id
    code
    customerTabId
    products {
      quantity
      totalPrice
      product {
        id
        name
        price
      }
    }
  }
}
```

## Testing DataLoader Efficiency

### Query 5: Multiple Orders (Tests N+1 Prevention)

This query will fetch multiple orders, each with multiple products. The DataLoader ensures that all products are loaded in a single batched query:

```graphql
query TestDataLoaderEfficiency {
  orders {
    id
    code
    products {
      productId
      quantity
      product {
        id
        name
        price
        needPreparation
      }
    }
  }
}
```

**Behind the scenes:**
1. Query fetches all orders
2. Query fetches all order products
3. DataLoader collects all unique product IDs
4. DataLoader makes ONE batched query: `SELECT * FROM Products WHERE Id IN (...)`
5. Results are cached and distributed to each order product

## Creating Test Data

### Mutation 1: Create a Product

```graphql
mutation CreateProduct {
  createProductAsync(
    name: "Test Burger"
    price: 15.99
    needPreparation: true
  ) {
    id
    name
    price
    code
    needPreparation
  }
}
```

### Mutation 2: Create an Order with Products

```graphql
mutation CreateOrder {
  createOrder(
    input: {
      products: [
        { productId: "YOUR_PRODUCT_ID_1", quantity: 2 }
        { productId: "YOUR_PRODUCT_ID_2", quantity: 1 }
      ]
    }
  ) {
    id
    code
    status
    products {
      productId
      quantity
      totalPrice
      product {
        id
        name
        price
      }
    }
  }
}
```

## Advanced Queries

### Query 6: Orders with Product Categories

```graphql
query GetOrdersWithProductCategories {
  orders {
    id
    code
    products {
      quantity
      totalPrice
      product {
        id
        name
        price
        productCategory {
          id
          name
          icon
        }
      }
    }
  }
}
```

### Query 7: Filter and Sort Orders

```graphql
query GetFilteredOrders {
  orders(where: { status: { eq: CREATED } }) {
    id
    code
    status
    products {
      quantity
      product {
        name
        price
      }
    }
  }
}
```

## Testing in GraphQL Playground

1. Open http://localhost:5012/graphql in your browser
2. Copy any of the queries above into the left panel
3. Click the "Play" button to execute
4. View the results in the right panel
5. Use the "Docs" tab to explore the schema

## Expected Response Format

When you query orders with products, you should receive a response like:

```json
{
  "data": {
    "orders": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "code": 1,
        "status": "CREATED",
        "products": [
          {
            "productId": "223e4567-e89b-12d3-a456-426614174001",
            "quantity": 2,
            "totalPrice": 31.98,
            "product": {
              "id": "223e4567-e89b-12d3-a456-426614174001",
              "name": "Cheeseburger",
              "price": 15.99,
              "code": 101,
              "needPreparation": true
            }
          }
        ]
      }
    ]
  }
}
```

## Performance Notes

- **DataLoader Batching**: All product queries are batched into a single database call per request
- **Caching**: Products are cached within a single request, so if the same product appears in multiple orders, it's only fetched once
- **Selective Loading**: GraphQL only fetches the fields you request, reducing payload size
- **Projection**: The `[UseProjection]` attribute ensures only requested fields are included in the SQL query

## Troubleshooting

If product information is not appearing:

1. **Check the query**: Make sure you're requesting the `product` field within `products`
2. **Verify data exists**: Run a simple products query to ensure products exist in the database
3. **Check logs**: Look for any errors in the application console
4. **Verify DataLoader registration**: Ensure `GetProductByIdDataLoader` is registered in DI

## Schema Exploration

Use the GraphQL Playground's "Docs" tab to explore:
- Available queries and mutations
- Field types and relationships
- Required vs optional fields
- Available filters and sorting options

