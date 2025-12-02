using ComandaX.Domain.Common;
using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Table : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public int Number { get; private set; }
    public TableStatusEnum Status { get; private set; } = TableStatusEnum.Free;

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }

    public void SetNumber(int number)
    {
        if (number < 0)
            throw new ArgumentException("Number must be greater or equal to zero", nameof(number));

        Number = number;
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
