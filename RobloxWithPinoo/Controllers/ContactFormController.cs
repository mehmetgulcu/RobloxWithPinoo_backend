using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.ContactFormDtos;
using RobloxWithPinoo.Services.ContactFormService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;

        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitContactForm([FromBody] CreateContactForm createContactForm)
        {
            try
            {
                if (createContactForm == null)
                {
                    return BadRequest("Form verileri geçerli değil.");
                }

                var success = await _contactFormService.SendContactForm(createContactForm);

                if (success)
                {
                    return Ok("Form başarıyla gönderildi.");
                }
                else
                {
                    return StatusCode(500, "Form gönderilirken bir hata oluştu.");
                }
            }
            catch (ArgumentException ex)
            {
                if (string.IsNullOrWhiteSpace(ex.ParamName))
                {
                    return BadRequest("Bir veya daha fazla gerekli alan boş bırakılmış.");
                }
                else if (ex.ParamName.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Geçerli bir e-posta adresi giriniz.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Bir hata oluştu. Form gönderilemedi.");
            }
        }

        [HttpGet("get-all-un-read-contact-forms")]
        public async Task<IActionResult> GetAllUnReadContacts()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var allForms = await _contactFormService.GetAllUnreadContacts(appUserId);

                return Ok(allForms);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "Bu işlemi yapmak için yetkiniz yok.")
                {
                    return Unauthorized("Bu işlemi yapmak için yetkiniz yok.");
                }
                else if (ex.Message == "Form bulunamadı.")
                {
                    return NotFound("Form bulunamadı.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("get-all-read-contact-forms")]
        public async Task<IActionResult> GetAllReadContacts()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var allForms = await _contactFormService.GetAllReadContacts(appUserId);

                return Ok(allForms);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "Bu işlemi yapmak için yetkiniz yok.")
                {
                    return Unauthorized("Bu işlemi yapmak için yetkiniz yok.");
                }
                else if (ex.Message == "Form bulunamadı.")
                {
                    return NotFound("Form bulunamadı.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpPatch("make-read-contact-forms/{formId}")]
        public async Task<IActionResult> MakeReadContactForm(Guid formId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _contactFormService.MakeReadContactForm(formId, appUserId);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "Bu işlemi yapmak için yetkiniz yok.")
                {
                    return Unauthorized("Bu işlemi yapmak için yetkiniz yok.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpDelete("delete-contact-forms/{formId}")]
        public async Task<IActionResult> DeleteContactForm(Guid formId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _contactFormService.DeleteContactForm(formId, appUserId);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "Bu işlemi yapmak için yetkiniz yok.")
                {
                    return Unauthorized("Bu işlemi yapmak için yetkiniz yok.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}
