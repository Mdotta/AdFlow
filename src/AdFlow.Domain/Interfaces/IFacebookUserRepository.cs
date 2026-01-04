using AdFlow.Domain.Entities;

namespace AdFlow.Domain.Interfaces;

public interface IFacebookUserRepository
{
    Task<FacebookUser?> GetByIdAsync(Guid id);
    Task<FacebookUser?> GetByFacebookIdAsync(string facebookId);
    Task<FacebookUser?> GetByEmailAsync(string email);
    Task<FacebookUser> CreateAsync(FacebookUser user);
    Task<FacebookUser> UpdateAsync(FacebookUser user);
    Task<IEnumerable<FacebookUser>> GetAllAsync();
}
