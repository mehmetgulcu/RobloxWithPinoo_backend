using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.ActivationCode;
using RobloxWithPinoo.Entity.Dtos.DocCategory;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.ActivationCodeService
{
    public class ActivationCodeService : IActivationCodeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ActivationCodeService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<ActivationCodeList>> ActivatedStates(Guid appUserId)
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

            var activeActivationCodes = await _context.ActivationCodes
                .Where(x => x.IsActive == true)
                .OrderByDescending(x => x.ActivatedDate)
                .ToListAsync();

            var codelistDtos = activeActivationCodes.Select(codes => new ActivationCodeList
            {
                Code = codes.Code,
                IsActive = codes.IsActive,
                ActivatedDate = codes.ActivatedDate.ToString(),
                ActivetedUserName = codes.ActivetedUserName,
                ActivetedUserLastName = codes.ActivetedUserLastName,
                CreatedDate = codes.CreatedDate.ToString(),
            }).ToList();

            return codelistDtos;
        }

        public async Task<bool> GenerateActivationCode(ActivationCodeNumber activationCodeNumber, Guid appUserId)
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

            for (int i = 0; i < activationCodeNumber.Amount; i++)
            {
                var newCode = new ActivationCode
                {
                    Code = Guid.NewGuid(),
                    IsActive = false,
                    ActivatedDate = null,
                    ActivetedUserName = null,
                    ActivetedUserLastName = null,
                    ActivatedUserId = null,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currentUser.UserName,
                    IsDeleted = false,
                };

                _context.ActivationCodes.Add(newCode);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ActivationCodeList>> NotActivatedStates(Guid appUserId)
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

            var activeActivationCodes = await _context.ActivationCodes
                .Where(x => x.IsActive == false)
                .OrderByDescending(x => x.CreatedDate) // Oluşturulma tarihine göre azalan şekilde sırala
                .ToListAsync();


            var codelistDtos = activeActivationCodes.Select(codes => new ActivationCodeList
            {
                Code = codes.Code,
                IsActive = codes.IsActive,
                ActivatedDate = codes.ActivatedDate.ToString(),
                ActivetedUserName = codes.ActivetedUserName,
                ActivetedUserLastName = codes.ActivetedUserLastName,
                CreatedDate = codes.CreatedDate.ToString(),
            }).ToList();

            return codelistDtos;
        }
    }
}
