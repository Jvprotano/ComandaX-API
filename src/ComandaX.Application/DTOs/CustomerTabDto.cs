using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class CustomerTabDto
{
    public CustomerTabDto()
    {
        Name = string.Empty;
    }

    public CustomerTabDto(Guid id, string name, Guid? tableId, CustomerTabStatusEnum status)
    {
        Id = id;
        Name = name;
        TableId = tableId;
        Status = status;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? TableId { get; set; }
    public CustomerTabStatusEnum Status { get; set; }
}
