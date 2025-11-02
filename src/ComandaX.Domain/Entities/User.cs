namespace ComandaX.Domain.Entities;

public class User : BaseEntity
{
    public User(string email, string role)
    {
        Email = email;
        Role = role;
    }
    public string Email { get; private set; }
    public string Role { get; private set; }
}
