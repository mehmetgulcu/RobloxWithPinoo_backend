
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.AccountDtos;
using RobloxWithPinoo.Entity.Entities;
using System.Text.Json;

namespace RobloxWithPinoo.Services.AccountService
{
    public class AccountService : IAccountService
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccountService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            // _context = context;
            _userManager = userManager;
        }

        public async Task<UserInfoViewDto> GetAccountInfoAsync(Guid appUserId)
        {
            try
            {
                var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

                if (currentUser == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                var userInfo = new UserInfoViewDto
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName
                };

                return userInfo;

            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }
                else
                {
                    Console.WriteLine(ex.Message.ToString());
                    throw new Exception("Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                }
            }
        }
    }
}
