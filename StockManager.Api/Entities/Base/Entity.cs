namespace StockManager.Api.Entities.Base;

public abstract class Entity(Guid id)
{
    protected Entity() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}