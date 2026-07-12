using StockManager.Api.Enums;
using StockManager.Api.Shared.Base;

namespace StockManager.Api.Models;

public sealed class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Password { get; private set; }
    public Guid CompanyId { get; }
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public User(string name, string email, string phone, string password, Guid companyId, Role role)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Password = password;
        CompanyId = companyId;
        Role = role;
    }

    public User(Guid id, string name, string email, string phone, string password, Guid companyId,
        Role role, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Password = password;
        CompanyId = companyId;
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}