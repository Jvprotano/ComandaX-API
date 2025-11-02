using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}
