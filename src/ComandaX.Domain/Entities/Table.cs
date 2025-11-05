using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Table : BaseEntity
{
    public int Code { get; private set; }
    public TableStatusEnum Status { get; private set; } = TableStatusEnum.Free;

    public void SetBusy()
    {
        Status = TableStatusEnum.Busy;
    }

    public void SetFree()
    {
        Status = TableStatusEnum.Free;
    }
}
