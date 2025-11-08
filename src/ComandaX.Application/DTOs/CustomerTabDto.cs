namespace ComandaX.Application.DTOs;

public sealed class CustomerTabDto
{
    public CustomerTabDto(Guid id, string? name, Guid? tableId)
    {
        this.Id = id;
        this.Name = name;
        this.TableId = tableId;
    }
    public CustomerTabDto()
    {

    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? TableId { get; set; }
    public TableDto? Table { get; set; }
}
