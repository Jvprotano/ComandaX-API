using Bogus;
using ComandaX.Domain.Entities;

namespace ComandaX.Tests.Helpers.Builders;

public class ProductBuilder
{
    private static readonly Faker<Product> _faker;

    static ProductBuilder()
    {
        _faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.NeedPreparation, f => f.Random.Bool(0.3f))
            .RuleFor(p => p.IsPricePerKg, f => f.Random.Bool(0.2f))
            .RuleFor(p => p.CreatedAt, f => f.Date.Past());
    }

    public static ProductBuilder New() => new();
    public Product Build() => _faker.Generate();
    public ProductBuilder WithId(Guid id) { _faker.RuleFor(p => p.Id, id); return this; }
    public ProductBuilder WithPrice(decimal price) { _faker.RuleFor(p => p.Price, price); return this; }
    public ProductBuilder WithName(string name) { _faker.RuleFor(p => p.Name, name); return this; }
    public ProductBuilder WithNeedPreparation(bool needPreparation) { _faker.RuleFor(p => p.NeedPreparation, needPreparation); return this; }
    public ProductBuilder WithPricePerKg(bool pricePerKg) { _faker.RuleFor(p => p.IsPricePerKg, pricePerKg); return this; }
    public ProductBuilder WithProductCategoryId(Guid? productCategoryId) { _faker.RuleFor(p => p.ProductCategoryId, productCategoryId); return this; }
    public List<Product> BuildList(int count = 5) => _faker.Generate(count);
}
