using StockManager.Api.Entities.Base;
using StockManager.Api.Entities.Enums;

namespace StockManager.Api.Entities.Models;

public sealed class User : Entity
{
    public User(string name, string email, string phone, string password, Guid companyId, Role role, bool passwordMustBeChanged)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Password = password;
        CompanyId = companyId;
        Role = role;
        PasswordMustBeChanged = passwordMustBeChanged;
    }

    public User(Guid id, string name, string email, string phone, string password, Guid companyId,
        Role role, bool passwordMustBeChanged, DateTime createdAt, DateTime updatedAt, bool active) : base(id)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Password = password;
        CompanyId = companyId;
        Role = role;
        PasswordMustBeChanged = passwordMustBeChanged;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Password { get; private set; }
    public Company? Company { get; private set; }
    public Guid CompanyId { get; }
    public Role Role { get; private set; }
    public bool PasswordMustBeChanged { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public bool Active { get; private set; } = true;

    public void SetName(string? newName)
    {
        if (string.IsNullOrEmpty(newName) || Name.Equals(newName)) return;
        Name = newName;
    }

    public void SetEmail(string? newEmail)
    {
        if (string.IsNullOrEmpty(newEmail) || Email.Equals(newEmail)) return;
        Email = newEmail;
    }

    public void SetPhone(string? newPhone)
    {
        if (string.IsNullOrEmpty(newPhone) || Phone.Equals(newPhone)) return;
        Phone = newPhone;
    }

    public void SetPassword(string? newPassword)
    {
        if (string.IsNullOrEmpty(newPassword) || Password.Equals(newPassword)) return;
        Password = newPassword;
    }

    public void SetUpdateAtToNow()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPasswordMustBeChangedToFalse()
    {
        PasswordMustBeChanged = false;
    }

    public void ToggleActive()
    {
        Active = !Active;
    }
}