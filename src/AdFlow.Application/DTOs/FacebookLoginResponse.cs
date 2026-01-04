namespace AdFlow.Application.DTOs;

public class FacebookLoginResponse
{
    public FacebookUserDto User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}
