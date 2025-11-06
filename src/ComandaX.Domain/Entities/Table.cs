using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Table : BaseEntity
{
    public int? Number { get; private set; }
    public TableStatusEnum Status { get; private set; } = TableStatusEnum.Free;

    public void SetBusy()
    {
        Status = TableStatusEnum.Busy;
    }

    public void SetNumber(int number)
    {
        Number = number;
    }

    public void SetFree()
    {
        Status = TableStatusEnum.Free;
    }
}
