using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Table : BaseEntity
{
    public Table(int code)
    {
        Code = code;
    }

    public Table()
    {
    }


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

    public void SetCode(int code)
    {
        Code = code;
    }
}
