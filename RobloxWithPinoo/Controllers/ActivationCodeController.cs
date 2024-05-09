using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.ActivationCode;
using RobloxWithPinoo.Services.ActivationCodeService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationCodeController : ControllerBase
    {
        private readonly IActivationCodeService _activationCodeService;

        public ActivationCodeController(IActivationCodeService activationCodeService)
        {
            _activationCodeService = activationCodeService;
        }

        [HttpPost("generate-activation-code")]
        public async Task<IActionResult> GenerateActivationCode(ActivationCodeNumber activationCodeNumber)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _activationCodeService.GenerateActivationCode(activationCodeNumber,appUserId);
                return Ok("Aktivasyon kodları üretildi.");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Bu işlemi yapmak için yetkiniz yok.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("activated-states")]
        public async Task<IActionResult> ActivatedStates()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var states = await _activationCodeService.ActivatedStates(appUserId);

                return Ok(states);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("not-activated-states")]
        public async Task<IActionResult> NotActivatedStates()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var states = await _activationCodeService.NotActivatedStates(appUserId);

                return Ok(states);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}
