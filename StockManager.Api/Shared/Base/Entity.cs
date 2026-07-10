namespace StockManager.Api.Shared.Base;

public abstract class Entity(Guid id)
{
    public Guid Id { get; } = id;

    protected Entity() : this(id: Guid.NewGuid()) { }
}