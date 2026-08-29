using StockManager.Api.Entities.Base;

namespace StockManager.Api.Entities.Models;

public sealed class Company : Entity
{
    public Company(string name, string cnpj)
    {
        Name = name;
        Cnpj = cnpj;
    }

    public Company(Guid id, string name, string cnpj, Guid ownerId, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        Name = name;    
        Cnpj = cnpj;
        OwnerId = ownerId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public string Name { get; private set; }
    public string Cnpj { get;  }
    public ICollection<User> Users { get; private set; } = [];
    public Guid OwnerId { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void SetOwnerId(Guid ownerId)
        => OwnerId = ownerId;
}