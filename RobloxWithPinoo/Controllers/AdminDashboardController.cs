using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Services.AdminDashboardService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet("get-total-doc-articles-count")]
        public async Task<IActionResult> GetTotalDocArticlesCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalDocArticlesCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-total-doc-categories-count")]
        public async Task<IActionResult> GetTotalDocCategoriesCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalDocCategoriesCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-category-article-count")]
        public async Task<IActionResult> GetCategoryArticleCounts()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetCategoryArticleCounts(appUserId);

                return Ok(count);
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

        [HttpGet("get-total-active-card-count")]
        public async Task<IActionResult> GetTotalActiveCardCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalActiveCardCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-total-users-count")]
        public async Task<IActionResult> GetTotalUsersCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalUsersCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-daily-register-chart")]
        public async Task<IActionResult> GetDailyRegisterChart()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetDailyRegisterChart(appUserId);

                return Ok(count);
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

        [HttpGet("get-weekly-register-chart")]
        public async Task<IActionResult> GetWeeklyRegisterChart()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetWeeklyRegisterChart(appUserId);

                return Ok(count);
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

        [HttpGet("get-monthly-register-chart")]
        public async Task<IActionResult> GetMonthlyRegisterChart()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetMonthlyRegisterChart(appUserId);

                return Ok(count);
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

        [HttpGet("get-yearly-register-chart")]
        public async Task<IActionResult> GetYearlyRegisterChart()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetYearlyRegisterChart(appUserId);

                return Ok(count);
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

        [HttpGet("get-generated-activation-codes-count")]
        public async Task<IActionResult> GetGeneratedActivationCodesCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetGeneratedActivationCodesCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-total-activated-codes-count")]
        public async Task<IActionResult> GetTotalActivatedCodesCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalActivatedCodesCount(appUserId);

                return Ok(count);
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

        [HttpGet("get-total-not-activated-codes-count")]
        public async Task<IActionResult> GetTotalNotActivatedCodesCount()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var count = await _adminDashboardService.GetTotalNotActivatedCodesCount(appUserId);

                return Ok(count);
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
