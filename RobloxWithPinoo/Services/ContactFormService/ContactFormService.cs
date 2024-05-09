using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.ContactFormDtos;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.ContactFormService
{
    public class ContactFormService : IContactFormService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactFormService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendContactForm(CreateContactForm createContactForm)
        {
            if (string.IsNullOrWhiteSpace(createContactForm.Name) ||
            string.IsNullOrWhiteSpace(createContactForm.Surname) ||
            string.IsNullOrWhiteSpace(createContactForm.Email) ||
            string.IsNullOrWhiteSpace(createContactForm.Message))
            {
                throw new ArgumentException("Tüm alanlar doldurulmalıdır.");
            }

            if (!IsValidEmail(createContactForm.Email))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi giriniz.");
            }

            var newContactForm = new ContactForm{
                Name = createContactForm.Name,
                Surname = createContactForm.Surname,
                Email = createContactForm.Email,
                Message = createContactForm.Message,
                IsRead = false,
                CreatedBy = createContactForm.Email,
                CreatedDate = DateTime.Now,
            };

            _context.ContactForms.Add(newContactForm);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AllContactForms>> GetAllUnreadContacts(Guid appUserId)
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

            var forms = await _context.ContactForms.Where(f => f.IsRead == false).ToListAsync();


            if (!forms.Any())
            {
                throw new KeyNotFoundException("Form bulunamadı.");
            }

            var formDtos = forms.Select(form => new AllContactForms
            {
                Id = form.Id,
                Name = form.Name,
                Surname = form.Surname,
                Email = form.Email,
                Message = form.Message,
                CreatedDate = form.CreatedDate.ToString("dd/MM/yyyy"),
                IsRead = form.IsRead,
            }).ToList();

            return formDtos;
        }

        public async Task<bool> DeleteContactForm(Guid formId, Guid appUserId)
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

            var form = await _context.ContactForms.FirstOrDefaultAsync(x => x.Id == formId);

            _context.ContactForms.Remove(form);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AllContactForms>> GetAllReadContacts(Guid appUserId)
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

            var forms = await _context.ContactForms.Where(f => f.IsRead == true).ToListAsync();


            if (!forms.Any())
            {
                throw new KeyNotFoundException("Form bulunamadı.");
            }

            var formDtos = forms.Select(form => new AllContactForms
            {
                Id = form.Id,
                Name = form.Name,
                Surname = form.Surname,
                Email = form.Email,
                Message = form.Message,
                CreatedDate = form.CreatedDate.ToString("dd/MM/yyyy"),
                IsRead = form.IsRead,
            }).ToList();

            return formDtos;
        }

        public async Task<bool> MakeReadContactForm(Guid formId, Guid appUserId)
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

            var form = await _context.ContactForms.FirstOrDefaultAsync(x => x.Id == formId);

            form.IsRead = true;
            form.ModifiedBy = currentUser.UserName;
            form.ModifiedDate = DateTime.Now;

            _context.ContactForms.Update(form);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
