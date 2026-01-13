using ComandaX.Domain.Entities;

namespace ComandaX.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Adds a new user to the data store.
    /// The actual save is performed when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
    /// </summary>
    Task<User> AddAsync(User user);
}
