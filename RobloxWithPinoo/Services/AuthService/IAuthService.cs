using Microsoft.AspNetCore.Identity;
using RobloxWithPinoo.Entity.Dtos.AuthDtos;

namespace RobloxWithPinoo.Services.AuthService
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
        Task<string> LoginUserAsync(LoginDto loginDto);
    }
}
