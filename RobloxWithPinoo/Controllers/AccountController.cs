using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Services.AccountService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("get-account-info")]
        public async Task<IActionResult> GetAccountInfo()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var userInfoValues = await _accountService.GetAccountInfoAsync(appUserId);

                return Ok(userInfoValues);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound(ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

    }
}
