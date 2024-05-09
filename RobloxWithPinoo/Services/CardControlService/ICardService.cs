using RobloxWithPinoo.Entity.Dtos.CardController;

namespace RobloxWithPinoo.Services.CardControlService
{
    public interface ICardService
    {
        Task<bool> CreateCardAsync(CreateCardDto createCardControlDto, Guid appUserId);
        Task<bool> UpdateSocketAsync(UpdateSocketModel model, Guid appUserId);
        Task<GetSocketValue[]> GetSocketValueAsync(string CardName, string SocketName, Guid appUserId);
        Task<List<CardListDto>> GetAllCardByAppUserId(Guid appUserId);
        Task<bool> DeleteCard(Guid cardId, Guid appUserId);
        Task<List<CardListForAdminDto>> GetAllCarsForAdmin(Guid appUserId);
    }
}
