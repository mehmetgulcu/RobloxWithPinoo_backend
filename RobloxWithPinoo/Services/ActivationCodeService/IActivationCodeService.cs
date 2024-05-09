using RobloxWithPinoo.Entity.Dtos.ActivationCode;
using RobloxWithPinoo.Entity.Dtos.DocCategory;

namespace RobloxWithPinoo.Services.ActivationCodeService
{
    public interface IActivationCodeService
    {
        Task<bool> GenerateActivationCode(ActivationCodeNumber activationCodeNumber,Guid appUserId);
        Task<List<ActivationCodeList>> ActivatedStates(Guid appUserId);
        Task<List<ActivationCodeList>> NotActivatedStates(Guid appUserId);
    }
}
