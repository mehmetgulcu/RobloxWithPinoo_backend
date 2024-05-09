using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.AuthDtos;
using RobloxWithPinoo.Entity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RobloxWithPinoo.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            string defaultRoleName = "User";

            if (!await _roleManager.RoleExistsAsync(defaultRoleName))
            {
                await _roleManager.CreateAsync(new AppRole { Name = defaultRoleName, NormalizedName = defaultRoleName.ToUpperInvariant(), ConcurrencyStamp = defaultRoleName });
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Bu e-posta adresiyle zaten bir kullanıcı mevcut." });
            }

            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                DateOfRegistration = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, defaultRoleName);
            }

            return result;
        }


        public async Task<string> LoginUserAsync(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);

			if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
			{
				var role = await _userManager.GetRolesAsync(user);
				return GenerateJwtToken(user.Id, role.FirstOrDefault());
			}

			return null;
		}

        public void SendEmail(string email, string subject, string messageBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Pinoo Robotics", "mehmetgulcudeveloper@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = messageBody };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("mehmetgulcudeveloper@gmail.com", "Mehmetakif104++**");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string GenerateJwtToken(Guid userId, string role)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
				new Claim(ClaimTypes.Name, userId.ToString()),
				new Claim(ClaimTypes.Role, role)
				}),
				Expires = DateTime.UtcNow.AddDays(5),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
