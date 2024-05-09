using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.CardController;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Dtos.DocCategory;
using RobloxWithPinoo.Services.DocCategoryService;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocCategoryController : ControllerBase
    {
        private readonly IDocCategoryService _docCategoryService;

        public DocCategoryController(IDocCategoryService docCategoryService)
        {
            _docCategoryService = docCategoryService;
        }

        [HttpPost("create-doc-category")]
        public async Task<IActionResult> AddDocCategory([FromBody] CreateDocCategoryDto createDocCategoryDto)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var docCategory = await _docCategoryService.CreateDocCategoryAsync(createDocCategoryDto, appUserId);

                return Created();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Böyle bir kategori zaten var.")
                {
                    return Conflict("Böyle bir kategori zaten var.");
                }
                else if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("get-doc-category-for-all-users")]
        public async Task<IActionResult> GetDocCategoryForAllUsers()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var allDocCategories = await _docCategoryService.GetAllDocCategoryForAllUser(appUserId);

                return Ok(allDocCategories);
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
                await _docCategoryService.UpdateIndexesAsync(sortedIds, appUserId);
                return Ok("Sıralama güncellendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<IActionResult> GetDocCategory(Guid id)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var category = await _docCategoryService.GetDocCategoryByIdAsync(id, appUserId);
                return Ok(category);
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

        [HttpPut("update-doc-category/{categoryId}")]
        public async Task<IActionResult> UpdateDocCategory(UpdateDocCategoryDto updateDocCategoryDto, Guid categoryId)
        {
            try
            {
                var appUserIdString = (string)HttpContext.Items["unique_name"];
                if (!Guid.TryParse(appUserIdString, out Guid appUserId))
                {
                    return Unauthorized("Geçersiz kullanıcı kimliği.");
                }

                if (updateDocCategoryDto == null)
                {
                    return BadRequest("Güncellenecek kategori bilgileri bulunamadı.");
                }

                var success = await _docCategoryService.UpdateDocCategoryAsync(updateDocCategoryDto, categoryId, appUserId);
                if (!success)
                {
                    return NotFound("Kategori bulunamadı veya güncellenemedi.");
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

        [HttpDelete("delete-doc-category/{categoryId}")]
        public async Task<IActionResult> DeleteDocCategory(Guid categoryId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _docCategoryService.DeleteDocCategoryAsync(categoryId, appUserId);

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
