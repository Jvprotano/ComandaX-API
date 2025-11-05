namespace ComandaX.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; internal set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }

    protected void EntityUpdated() => UpdatedAt = DateTime.UtcNow;
}
