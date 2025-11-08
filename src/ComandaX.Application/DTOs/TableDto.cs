using ComandaX.Domain.Enums;

namespace ComandaX.Application.DTOs;

public sealed class TableDto
{
    public TableDto(Guid id, int? number, TableStatusEnum status)
    {
        this.Id = id;
        this.Number = number;
        this.Status = status;
    }
    public TableDto()
    {

    }

    public Guid Id { get; set; }
    public int? Number { get; set; }
    public TableStatusEnum Status { get; set; }
};
