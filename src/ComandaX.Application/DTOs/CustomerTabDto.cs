using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class CustomerTabDto
{
    public CustomerTabDto(Guid id, string? name, Guid? tableId, CustomerTabEnum status, List<Guid>? orderIds = null)
    {
        Id = id;
        Name = name;
        TableId = tableId;
        Status = status;
        OrderIds = orderIds;
    }
    public CustomerTabDto()
    {

    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? TableId { get; set; }
    public TableDto? Table { get; set; }
    public List<Guid>? OrderIds { get; set; }
    public List<OrderDto>? Orders { get; set; }
    public CustomerTabEnum Status { get; set; }
}
