using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class TableDto
{
    public TableDto() { }
    public TableDto(Guid id, int number, TableStatusEnum status)
    {
        Id = id;
        Number = number;
        Status = status;
    }

    public Guid Id { get; set; }
    public int Number { get; set; }
    public TableStatusEnum Status { get; set; }
};
