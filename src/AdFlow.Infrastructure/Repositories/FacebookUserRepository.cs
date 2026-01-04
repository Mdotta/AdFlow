using AdFlow.Domain.Entities;
using AdFlow.Domain.Interfaces;
using AdFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdFlow.Infrastructure.Repositories;

public class FacebookUserRepository : IFacebookUserRepository
{
    private readonly AdFlowDbContext _context;

    public FacebookUserRepository(AdFlowDbContext context)
    {
        _context = context;
    }

    public async Task<FacebookUser?> GetByIdAsync(Guid id)
    {
        return await _context.FacebookUsers.FindAsync(id);
    }

    public async Task<FacebookUser?> GetByFacebookIdAsync(string facebookId)
    {
        return await _context.FacebookUsers
            .FirstOrDefaultAsync(u => u.FacebookId == facebookId);
    }

    public async Task<FacebookUser?> GetByEmailAsync(string email)
    {
        return await _context.FacebookUsers
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<FacebookUser> CreateAsync(FacebookUser user)
    {
        _context.FacebookUsers.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<FacebookUser> UpdateAsync(FacebookUser user)
    {
        _context.FacebookUsers.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<FacebookUser>> GetAllAsync()
    {
        return await _context.FacebookUsers
            .Where(u => u.IsActive)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }
}
