using RobloxWithPinoo.Entity.Dtos.AccountDtos;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.AccountService
{
    public interface IAccountService
    {
        Task<UserInfoViewDto> GetAccountInfoAsync(Guid appUserId);
    }
}