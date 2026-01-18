namespace ComandaX.Application.DTOs;

public sealed class ProductDto
{
    public ProductDto() { }

    public ProductDto(
        Guid id,
        string name,
        decimal price,
        int code,
        bool needPreparation,
        bool pricePerKg,
        Guid? productCategoryId)
    {
        Id = id;
        Name = name;
        Price = price;
        Code = code;
        NeedPreparation = needPreparation;
        IsPricePerKg = pricePerKg;
        ProductCategoryId = productCategoryId;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Code { get; set; }
    public bool NeedPreparation { get; set; }
    public Guid? ProductCategoryId { get; set; }
    public ProductCategoryDto? ProductCategory { get; set; }
    public bool IsPricePerKg { get; set; }
}