namespace ComandaX.Domain.Entities;

public class Tab : BaseEntity
{
    public Tab(string name, Guid? tableId)
    {
        Name = name;
        TableId = tableId;
    }

    public string Name { get; private set; }
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }

    public IList<Order>? Orders { get; set; }
}
