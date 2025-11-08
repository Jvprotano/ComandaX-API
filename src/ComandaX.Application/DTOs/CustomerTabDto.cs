using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class CustomerTabDto
{
    public CustomerTabDto(Guid id, string? name, Guid? tableId, CustomerTabEnum status)
    {
        Id = id;
        Name = name;
        TableId = tableId;
        Status = status;
    }
    public CustomerTabDto()
    {

    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? TableId { get; set; }
    public TableDto? Table { get; set; }
    public CustomerTabEnum Status { get; set; }
}
