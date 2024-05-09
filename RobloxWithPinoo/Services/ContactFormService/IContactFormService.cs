using RobloxWithPinoo.Entity.Dtos.ContactFormDtos;

namespace RobloxWithPinoo.Services.ContactFormService
{
    public interface IContactFormService
    {
        Task<bool> SendContactForm(CreateContactForm createContactForm);
        Task<List<AllContactForms>> GetAllUnreadContacts(Guid appUserId);
        Task<List<AllContactForms>> GetAllReadContacts(Guid appUserId);
        Task<bool> MakeReadContactForm(Guid formId, Guid appUserId);
        Task<bool> DeleteContactForm(Guid formId, Guid appUserId);
    }
}
