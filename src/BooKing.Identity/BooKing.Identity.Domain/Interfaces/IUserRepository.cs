using BooKing.Identity.Domain.Entities;

namespace BooKing.Identity.Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailAlreadyInUse(string email);
}
