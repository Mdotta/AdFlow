namespace AdFlow.Application.DTOs;

public class FacebookUserDto
{
    public Guid Id { get; set; }
    public string FacebookId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
