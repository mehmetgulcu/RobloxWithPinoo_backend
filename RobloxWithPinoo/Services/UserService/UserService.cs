using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Dtos.UserDtos;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<ListUsersDto>> GetAllUsers(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!_userManager.IsInRoleAsync(currentUser, "Admin").Result)
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var users = await _context.Users
                .ToListAsync();

            var userDtos = users.Select(user => new ListUsersDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfRegistration = user.DateOfRegistration.ToString("dd/MM/yyyy"),
            }).ToList();

            return userDtos;
        }
    }
}
