namespace TechTrack.AuthService.Logic.Models
{
    public record RegisterDto(string Email, string FullName, string Password, string PhoneNumber);
}
