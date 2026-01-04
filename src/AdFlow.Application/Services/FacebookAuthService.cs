using AdFlow.Application.DTOs;
using AdFlow.Application.Interfaces;
using AdFlow.Domain.Entities;
using AdFlow.Domain.Interfaces;

namespace AdFlow.Application.Services;

public class FacebookAuthService : IFacebookAuthService
{
    private readonly IFacebookUserRepository _userRepository;
    private readonly IFacebookApiClient _facebookApiClient;
    private readonly IJwtTokenService _jwtTokenService;

    public FacebookAuthService(
        IFacebookUserRepository userRepository,
        IFacebookApiClient facebookApiClient,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _facebookApiClient = facebookApiClient;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<FacebookLoginResponse> LoginAsync(FacebookLoginRequest request)
    {
        // Validate Facebook access token and get user info
        var fbUserInfo = await _facebookApiClient.GetUserInfoAsync(request.AccessToken);
        if (fbUserInfo == null)
        {
            throw new UnauthorizedAccessException("Invalid Facebook access token");
        }

        // Check if user already exists
        var existingUser = await _userRepository.GetByFacebookIdAsync(fbUserInfo.Id);
        
        FacebookUser user;
        if (existingUser == null)
        {
            // Create new user
            user = new FacebookUser
            {
                Id = Guid.NewGuid(),
                FacebookId = fbUserInfo.Id,
                Email = fbUserInfo.Email,
                Name = fbUserInfo.Name,
                ProfilePictureUrl = fbUserInfo.Picture?.Data?.Url,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                IsActive = true
            };
            user = await _userRepository.CreateAsync(user);
        }
        else
        {
            // Update last login
            existingUser.LastLoginAt = DateTime.UtcNow;
            user = await _userRepository.UpdateAsync(existingUser);
        }

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email);

        return new FacebookLoginResponse
        {
            User = MapToDto(user),
            Token = token
        };
    }

    public async Task<FacebookUserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<IEnumerable<FacebookUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    private FacebookUserDto MapToDto(FacebookUser user)
    {
        return new FacebookUserDto
        {
            Id = user.Id,
            FacebookId = user.FacebookId,
            Email = user.Email,
            Name = user.Name,
            ProfilePictureUrl = user.ProfilePictureUrl,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
