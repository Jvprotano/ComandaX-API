namespace ComandaX.Application.Exceptions;

public class OrderWithoutProductsException() : Exception("Order must have at least one product");
