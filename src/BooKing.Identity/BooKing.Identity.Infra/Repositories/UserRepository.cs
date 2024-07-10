using BooKing.Generics.Infra;
using BooKing.Identity.Domain.Entities;
using BooKing.Identity.Domain.Interfaces;
using BooKing.Identity.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Identity.Infra.Repositories;
public class UserRepository : IUserRepository
{
    private readonly BooKingIdentityContext _context;
    private readonly IUnitOfWork<BooKingIdentityContext> _unitOfWork;

    public UserRepository(BooKingIdentityContext context, IUnitOfWork<BooKingIdentityContext> unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);

        await _unitOfWork.CommitAsync();

        return user;
    }

    public async Task<bool> EmailAlreadyInUse(string email)
    {
        return await _context.Set<User>().AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
                             .Include(u => u.UserSalt)
                             .Where(u => u.Email.Equals(email))
                             .FirstOrDefaultAsync();
    }
}
