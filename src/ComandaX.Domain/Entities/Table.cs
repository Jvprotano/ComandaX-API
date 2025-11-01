using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Table
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Code { get; set; }
    public TableStatusEnum Status { get; set; } = TableStatusEnum.Free;

    public Table(int code)
    {
        Code = code;
    }

    public void SetBusy()
    {
        Status = TableStatusEnum.Busy;
    }
    public void SetFree()
    {
        Status = TableStatusEnum.Free;
    }
}
