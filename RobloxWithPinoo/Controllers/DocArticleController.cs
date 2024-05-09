using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Services.DocArticleService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocArticleController : ControllerBase
    {
        private readonly IDocArticleService _docArticleService;

        public DocArticleController(IDocArticleService docArticleService)
        {
            _docArticleService = docArticleService;
        }

        [HttpPost("create-doc-article")]
        public async Task<IActionResult> CreateDocArticle([FromBody] CreateDocArticleDto createDocArticleDto)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                if (createDocArticleDto.ImageUrl == null || createDocArticleDto.ImageUrl.Length == 0)
                {
                    return BadRequest("Lütfen bir görüntü yükleyin.");
                }

                await _docArticleService.CreateDocArticleAsync(createDocArticleDto, appUserId);
                return Ok("Makale oluşturuldu.");
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

        [HttpGet("get-article/{id}")]
        public async Task<IActionResult> GetDocArticle(Guid id)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var article = await _docArticleService.GetDocArticleByIdAsync(id, appUserId);
                return Ok(article);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Makale getirilirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("get-articles-by-category/{categoryId}")]
        public async Task<IActionResult> GetDocArticlesByCategory(Guid categoryId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var articles = await _docArticleService.GetDocArticleByCategoryAsync(categoryId, appUserId);
                return Ok(articles);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Makale getirilirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("get-all-doc-articles")]
        public async Task<IActionResult> GetAllDocArticles()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var articles = await _docArticleService.GetAllDocArticles(appUserId);
                return Ok(articles);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Makale getirilirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("UpdateIndexes")]
        public async Task<IActionResult> UpdateIndexes(List<Guid> sortedIds)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            if (sortedIds == null || sortedIds.Count == 0)
            {
                return BadRequest("Sıralanmış Id'ler boş.");
            }

            try
            {
                await _docArticleService.UpdateIndexesAsync(sortedIds, appUserId);
                return Ok("Sıralama güncellendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPut("update-doc-article/{articleId}")]
        public async Task<IActionResult> UpdateDocArticle([FromBody] UpdateDocArticle updateDocArticle, Guid articleId)
        {
            try
            {
                var appUserIdString = (string)HttpContext.Items["unique_name"];
                if (!Guid.TryParse(appUserIdString, out Guid appUserId))
                {
                    return Unauthorized("Geçersiz kullanıcı kimliği.");
                }

                if (updateDocArticle == null)
                {
                    return BadRequest("Güncellenecek makale bilgileri bulunamadı.");
                }

                if (updateDocArticle.ImageUrl == null || updateDocArticle.ImageUrl.Length == 0)
                {
                    return BadRequest("Lütfen bir görüntü yükleyin.");
                }

                var success = await _docArticleService.UpdateDocArticleAsync(updateDocArticle, articleId, appUserId);
                if (!success)
                {
                    return NotFound("Makale bulunamadı veya güncellenemedi.");
                }

                return NoContent();
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

        [HttpDelete("delete-doc-article/{articleId}")]
        public async Task<IActionResult> DeleteDocArticle(Guid articleId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _docArticleService.DeleteDocArticleAsync(articleId, appUserId);
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
