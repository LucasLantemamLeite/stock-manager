namespace StockManager.Api.Entities.Base;

public abstract class Entity(Guid id)
{
    public Guid Id { get; } = id;

    protected Entity() : this(id: Guid.NewGuid()) { }
}