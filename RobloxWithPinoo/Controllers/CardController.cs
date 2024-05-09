using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobloxWithPinoo.Entity.Dtos.CardController;
using RobloxWithPinoo.Entity.Entities;
using RobloxWithPinoo.Services.CardControlService;
using System.Web;

namespace RobloxWithPinoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("create-card")]
        public async Task<IActionResult> AddCardControl([FromBody] CreateCardDto createCardDto)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var project = await _cardService.CreateCardAsync(createCardDto, appUserId);
                return Ok("Başarılı");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Böyle bir kart zaten var.")
                {
                    return Conflict("Böyle bir kart zaten var.");
                }
                else if (ex.Message == "Aktivasyon kodu geçersiz veya zaten kullanılmış.")
                {
                    return NotFound("Aktivasyon kodu geçersiz veya zaten kullanılmış.");
                }
                else if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "Kart adı boşluk veya Türkçe karakter içeremez.")
                {
                    return BadRequest("Kart adı boşluk veya Türkçe karakter içeremez.");
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpPatch("update-socket")]
        public async Task<IActionResult> UpdateSocket([FromBody] UpdateSocketModel model)
        {
            try
            {
                var appUserIdString = (string)HttpContext.Items["unique_name"];

                if (!Guid.TryParse(appUserIdString, out Guid appUserId))
                {
                    return Unauthorized("Geçersiz kullanıcı kimliği.");
                }

                var result = await _cardService.UpdateSocketAsync(model, appUserId);

                if (result)
                {
                    return Ok($"{model.SocketName} başarıyla güncellendi.");
                }
                else
                {
                    return NotFound("Veri bulunamadı veya geçersiz socket adı.");
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "İlgili kart bulunamadı.")
                {
                    return NotFound("İlgili kart bulunamadı.");
                }
                else if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                Console.WriteLine(ex.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("get-socket-value")]
        public async Task<IActionResult> GetSocketValue([FromQuery(Name = "CardName")] string CardName, [FromQuery(Name = "SocketName")] string SocketName)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var result = await _cardService.GetSocketValueAsync(CardName, SocketName, appUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "İlgili kart bulunamadı." || ex.Message == "Kullanıcı bulunamadı." || ex.Message == "Geçersiz soket adı.")
                {
                    return NotFound(ex.Message);
                }

                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }

        [HttpGet("get-all-card-by-appuser")]
        public async Task<IActionResult> GetAllCardByAppUserId()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var cardList = await _cardService.GetAllCardByAppUserId(appUserId);
                return Ok(cardList);
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

        [HttpDelete("delete-card/{cardId}")]
        public async Task<IActionResult> DeleteCard(Guid cardId)
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                await _cardService.DeleteCard(cardId, appUserId);

                return NoContent();
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

        [HttpGet("get-all-cards-for-admin")]
        public async Task<IActionResult> GetAllCardsForAdmin()
        {
            var appUserIdString = (string)HttpContext.Items["unique_name"];

            if (!Guid.TryParse(appUserIdString, out Guid appUserId))
            {
                return Unauthorized("Geçersiz kullanıcı kimliği.");
            }

            try
            {
                var allCards = await _cardService.GetAllCarsForAdmin(appUserId);
                return Ok(allCards);
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
