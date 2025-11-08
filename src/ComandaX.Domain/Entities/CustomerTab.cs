using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public class CustomerTab : BaseEntity
{
    public CustomerTab()
    {
    }

    public CustomerTab(string? name, Guid? tableId = null)
    {
        Name = name;
        TableId = tableId;
    }
    public string? Name { get; private set; }
    public CustomerTabEnum Status { get; private set; } = CustomerTabEnum.Open;
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }

    public IList<Order>? Orders { get; set; }

    public void Close()
    {
        Status = CustomerTabEnum.Closed;
        EntityUpdated();
    }
}
