namespace ComandaX.Domain.Entities;

public sealed class Product(string name, decimal price)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public decimal Price { get; private set; } = price;
    public int Code { get; private set; }

    public void SetNextCode(int lastCode)
    {
        Code = lastCode + 1;
    }
}
