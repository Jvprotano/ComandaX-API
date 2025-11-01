namespace ComandaX.Domain.Entities;

public class Tab
{
    public Tab(string name, Guid? tableId)
    {
        Id = Guid.NewGuid();
        Name = name;
        TableId = tableId;
    }

    public Guid Id { get; }
    public string Name { get; private set; }
    public Guid? TableId { get; private set; }
    public Table? Table { get; set; }

    public IList<Order>? Orders { get; set; }
}
