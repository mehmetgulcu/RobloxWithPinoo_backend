using RobloxWithPinoo.Entity.Dtos.UserDtos;

namespace RobloxWithPinoo.Services.UserService
{
    public interface IUserService
    {
        Task<List<ListUsersDto>> GetAllUsers(Guid appUserId);
    }
}
