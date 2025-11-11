using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class CustomerTabDto
{
    public CustomerTabDto()
    {
        Name = string.Empty;
    }

    public CustomerTabDto(Guid id, string name, Guid? tableId, CustomerTabStatusEnum status, int code)
    {
        Id = id;
        Name = name;
        TableId = tableId;
        Status = status;
        Code = code;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? TableId { get; set; }
    public CustomerTabStatusEnum Status { get; set; }
    public int Code { get; set; }
}
