using ComandaX.Domain.Common;
using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public class CustomerTab : BaseEntity, ITenantEntity
{
    public CustomerTab()
    {
        Name = string.Empty;
    }

    public CustomerTab(string name, Guid? tableId = null)
    {
        Name = name;
        TableId = tableId;
    }

    public Guid TenantId { get; private set; }
    public int Code { get; private set; }
    public string Name { get; private set; }
    public CustomerTabStatusEnum Status { get; private set; } = CustomerTabStatusEnum.Open;
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }
    public ICollection<Order> Orders { get; private set; } = [];

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));

        TenantId = tenantId;
    }

    public void Close()
    {
        Status = CustomerTabStatusEnum.Closed;
        EntityUpdated();
    }
}
