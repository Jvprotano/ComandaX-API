namespace ComandaX.Domain.Entities;

public sealed class Product : BaseEntity
{
    public Product(string name, decimal price, int code)
    {
        Name = name;
        Price = price;
        Code = code;
    }

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Code { get; private set; }

    public void SetCode(int code)
    {
        Code = code;
    }
}
