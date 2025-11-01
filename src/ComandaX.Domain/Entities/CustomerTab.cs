namespace ComandaX.Domain.Entities;

public class CustomerTab : BaseEntity
{
    public CustomerTab()
    {
    }

    public CustomerTab(string? name, Guid? tableId = null)
    {
        this.Name = name;
        this.TableId = tableId;
    }
    public string? Name { get; private set; }
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }

    public IList<Order>? Orders { get; set; }
}
