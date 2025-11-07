namespace ComandaX.Domain.Entities;

public class User(string email, string role) : BaseEntity
{
    public string Email { get; private set; } = email;
    public string Role { get; private set; } = role;
}
