using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.CardController;
using RobloxWithPinoo.Entity.Entities;
using System.ComponentModel;
using System.Text.Json;

namespace RobloxWithPinoo.Services.CardControlService
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CardService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private bool ContainsTurkishCharacter(string input)
        {
            string turkishCharacters = "çÇğĞıİöÖşŞüÜ";
            foreach (char c in input)
            {
                if (turkishCharacters.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CreateCardAsync(CreateCardDto createCardControlDto, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (createCardControlDto.CardName.Contains(" ") || ContainsTurkishCharacter(createCardControlDto.CardName))
            {
                throw new Exception("Kart adı boşluk veya Türkçe karakter içeremez.");
            }

            var existingCategory = await _context.Cards
                .FirstOrDefaultAsync(c => c.CardName == createCardControlDto.CardName && c.AppUserId == appUserId);

            if (existingCategory != null)
            {
                throw new Exception("Böyle bir kart zaten var.");
            }

            var activationCodeEntity = await _context.ActivationCodes.FirstOrDefaultAsync(x => x.Code == createCardControlDto.ActivationCode && !x.IsActive);

            if (activationCodeEntity != null)
            {
                activationCodeEntity.IsActive = true;
                activationCodeEntity.ActivatedDate = DateTime.UtcNow;
                activationCodeEntity.ActivetedUserName = currentUser.FirstName;
                activationCodeEntity.ActivetedUserLastName = currentUser.LastName;
                activationCodeEntity.ActivatedUserId = currentUser.Id;

                var newCardController = new Card
                {
                    CardName = createCardControlDto.CardName,
                    AppUserId = appUserId,
                    CreatedBy = currentUser.UserName,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                };

                _context.Cards.Add(newCardController);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                throw new Exception("Aktivasyon kodu geçersiz veya zaten kullanılmış.");
            }
        }


        public async Task<bool> DeleteCard(Guid cardId, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(c => c.Id == cardId && c.AppUserId == appUserId);

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CardListDto>> GetAllCardByAppUserId(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var cardControl = await _context.Cards
                .Where(c => c.AppUserId == appUserId)
                .ToListAsync();

            var cardListDto = cardControl.Select(c => new CardListDto
            {
                Id = c.Id,
                CardName = c.CardName,
                ModifiedDate = c.ModifiedDate.ToString(),
                CreatedDate = c.CreatedDate.ToString("dd/MM/yyyy"),
            }).ToList();

            return cardListDto;
        }

        public async Task<List<CardListForAdminDto>> GetAllCarsForAdmin(Guid appUserId)
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

            var cardControl = await _context.Cards
                .Include(x => x.AppUser)
                .ToListAsync();

            var allCardsListDto = cardControl.Select(c => new CardListForAdminDto
            {
                Id = c.Id,
                CardName = c.CardName,
                ModifiedDate = c.ModifiedDate.ToString(),
                CreatedDate = c.CreatedDate.ToString("dd/MM/yyyy"),
                FirstLastName = c.AppUser.FirstName + " " + c.AppUser.LastName,
                Mail = c.AppUser.UserName,
                UserRegisterdDate = c.AppUser.DateOfRegistration.ToString("dd/MM/yyyy"),
            }).ToList();

            return allCardsListDto;

        }

        public async Task<GetSocketValue[]> GetSocketValueAsync(string CardName, string SocketName, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var cardControl = await _context.Cards
                .Where(c => c.CardName == CardName && c.AppUserId == appUserId)
                .FirstOrDefaultAsync();

            if (cardControl == null)
            {
                throw new Exception("İlgili kart bulunamadı.");
            }

            object socketValue = null;

            switch (SocketName)
            {
                case nameof(Card.Pinoo1):
                    socketValue = cardControl.Pinoo1;
                    break;
                case nameof(Card.Pinoo2):
                    socketValue = cardControl.Pinoo2;
                    break;
                case nameof(Card.Pinoo3):
                    socketValue = cardControl.Pinoo3;
                    break;
                case nameof(Card.Pinoo4):
                    socketValue = cardControl.Pinoo4;
                    break;
                case nameof(Card.Pinoo5):
                    socketValue = cardControl.Pinoo5;
                    break;
                case nameof(Card.Pinoo6):
                    socketValue = cardControl.Pinoo6;
                    break;
                case nameof(Card.Pinoo7):
                    socketValue = cardControl.Pinoo7;
                    break;
                case nameof(Card.Pinoo8):
                    socketValue = cardControl.Pinoo8;
                    break;
                case nameof(Card.Pinoo9):
                    socketValue = cardControl.Pinoo9;
                    break;
                case nameof(Card.Pinoo10):
                    socketValue = cardControl.Pinoo10;
                    break;
                default:
                    throw new Exception("Geçersiz soket adı.");
            }

            return new GetSocketValue[] { new GetSocketValue { SocketValue = socketValue } };
        }

        public async Task<bool> UpdateSocketAsync(UpdateSocketModel model, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var cardControl = await _context.Cards.FirstOrDefaultAsync(c => c.CardName == model.CardName);

            if (cardControl == null)
            {
                throw new Exception("İlgili kart bulunamadı.");
            }

            try
            {
                switch (model.SocketName)
                {
                    case nameof(Card.Pinoo1):
                        if (model.SocketValue is JsonElement jsonElementPinoo1 && jsonElementPinoo1.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo1 = jsonElementPinoo1.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo2):
                        if (model.SocketValue is JsonElement jsonElementPinoo2 && jsonElementPinoo2.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo2 = jsonElementPinoo2.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo3):
                        if (model.SocketValue is JsonElement jsonElementPinoo3 && jsonElementPinoo3.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo3 = jsonElementPinoo3.GetDouble();
                        }
                        else
                        {
                            throw new Exception("Socket değeri double'a dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo4):
                        if (model.SocketValue is JsonElement jsonElementPinoo4 && jsonElementPinoo4.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo4 = jsonElementPinoo4.GetDouble();
                        }
                        else
                        {
                            throw new Exception("Socket değeri double'a dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo5):
                        if (model.SocketValue is JsonElement jsonElementPinoo5 && jsonElementPinoo5.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo5 = jsonElementPinoo5.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo6):
                        if (model.SocketValue is JsonElement jsonElementPinoo6 && jsonElementPinoo6.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo6 = jsonElementPinoo6.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo7):
                        if (model.SocketValue is JsonElement jsonElementPinoo7 && jsonElementPinoo7.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo7 = jsonElementPinoo7.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo8):
                        if (model.SocketValue is JsonElement jsonElementPinoo8 && jsonElementPinoo8.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo8 = jsonElementPinoo8.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo9):
                        if (model.SocketValue is JsonElement jsonElementPinoo9 && jsonElementPinoo9.ValueKind == JsonValueKind.Number)
                        {
                            cardControl.Pinoo9 = jsonElementPinoo9.GetInt32();
                        }
                        else
                        {
                            throw new Exception("Socket değeri int'e dönüştürülemedi.");
                        }
                        break;
                    case nameof(Card.Pinoo10):
                        if (model.SocketValue is JsonElement jsonElementPinoo10 && jsonElementPinoo10.ValueKind == JsonValueKind.String)
                        {
                            string stringValue = jsonElementPinoo10.ToString();
                            cardControl.Pinoo10 = stringValue;
                        }
                        else
                        {
                            throw new Exception("Socket değeri string'e dönüştürülemedi.");
                        }
                        break;
                    default:
                        return false;
                }

                cardControl.ModifiedBy = currentUser.UserName;
                cardControl.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Kullanıcı bulunamadı.")
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }
                else if (ex.Message == "İlgili kart bulunamadı.")
                {
                    throw new Exception("İlgili kart bulunamadı.");
                }
                else
                {
                    throw new Exception("Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                }
            }
        }

    }
}
