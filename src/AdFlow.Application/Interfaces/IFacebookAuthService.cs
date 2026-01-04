using AdFlow.Application.DTOs;

namespace AdFlow.Application.Interfaces;

public interface IFacebookAuthService
{
    Task<FacebookLoginResponse> LoginAsync(FacebookLoginRequest request);
    Task<FacebookUserDto?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<FacebookUserDto>> GetAllUsersAsync();
}
