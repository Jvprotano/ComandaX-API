namespace ComandaX.Application.DTOs;

public sealed record ProductCategoryDto
{
    public ProductCategoryDto() { }

    public ProductCategoryDto(Guid id, string name, string? icon)
    {
        Id = id;
        Name = name;
        Icon = icon;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
}
