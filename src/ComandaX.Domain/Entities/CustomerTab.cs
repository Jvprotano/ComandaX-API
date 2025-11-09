using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public class CustomerTab : BaseEntity
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
    public string Name { get; private set; }
    public CustomerTabStatusEnum Status { get; private set; } = CustomerTabStatusEnum.Open;
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }

    public void Close()
    {
        Status = CustomerTabStatusEnum.Closed;
        EntityUpdated();
    }
}
