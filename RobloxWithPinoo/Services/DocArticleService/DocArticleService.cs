using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.DocArticleService
{
    public class DocArticleService : IDocArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DocArticleService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> CreateDocArticleAsync(CreateDocArticleDto createDocArticleDto, Guid appUserId)
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

            var newDocArticle = new DocArticle
            {
                Title = createDocArticleDto.Title,
                Content = createDocArticleDto.Content,
                ImageUrl = createDocArticleDto.ImageUrl,
                DocCategoryId = createDocArticleDto.DocCategoryId,
                AppUserId = appUserId,
                CreatedBy = currentUser.UserName,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            };

            _context.DocArticles.Add(newDocArticle);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDocArticleAsync(Guid articleId, Guid appUserId)
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

            var article = await _context.DocArticles.FirstOrDefaultAsync(x => x.Id == articleId);

            _context.DocArticles.Remove(article);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ListDocArticles>> GetAllDocArticles(Guid appUserId)
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

            var articles = await _context.DocArticles
                .Include(article => article.DocCategory)
                .Where(article => article.DocCategory != null)
                .OrderBy(article => article.IndexNo)
                .ToListAsync();


            if (!articles.Any())
            {
                throw new KeyNotFoundException("Makale bulunamadı.");
            }

            var articleDtos = articles.Select(article => new ListDocArticles
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                DocCategoryName = article.DocCategory.Name,
                CreatedDate = article.CreatedDate.ToString("dd/MM/yyyy"),
                IndexNo = article.IndexNo
            }).ToList();

            return articleDtos;
        }

        public async Task<List<DocArticleDto>> GetDocArticleByCategoryAsync(Guid categoryId, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var articles = await _context.DocArticles
            .Include(article => article.DocCategory)
            .Where(x => x.DocCategoryId == categoryId)
            .OrderBy(x => x.IndexNo)
            .ToListAsync();


            if (!articles.Any())
            {
                throw new KeyNotFoundException("Makale bulunamadı.");
            }

            var articleDtos = articles.Select(article => new DocArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                DocCategoryId = categoryId,
                DocCategoryName = article.DocCategory.Name,
                CreatedDate = article.CreatedDate.ToString("dd/MM/yyyy"),
                IndexNo = article.IndexNo
            }).ToList();

            return articleDtos;
        }

        public async Task<DocArticleDto> GetDocArticleByIdAsync(Guid articleId, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var article = await _context.DocArticles
                .Include(a => a.DocCategory)
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null)
            {
                throw new KeyNotFoundException("Makale bulunamadı.");
            }

            var articleDto = new DocArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                DocCategoryId = article.DocCategoryId,
                CreatedDate = article.CreatedDate.ToString("dd/MM/yyyy"),
                IndexNo = article.IndexNo,
                DocCategoryName = article.DocCategory.Name
            };

            return articleDto;
        }

        public async Task<bool> UpdateDocArticleAsync(UpdateDocArticle updateDocArticle, Guid articleId, Guid appUserId)
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

            var article = await _context.DocArticles
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article != null)
            {

                article.Id = articleId;
                article.Title = updateDocArticle.Title;
                article.Content = updateDocArticle.Content;
                article.ImageUrl = updateDocArticle.ImageUrl;
                article.DocCategoryId = updateDocArticle.DocCategoryId;
                article.IndexNo = article.IndexNo;
                article.AppUserId = appUserId;
                article.ModifiedBy = currentUser.UserName;
                article.ModifiedDate = DateTime.Now;
                article.IsDeleted = false;

                _context.DocArticles.Update(article);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task UpdateIndexesAsync(List<Guid> sortedIds, Guid appUserId)
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

            for (int i = 0; i < sortedIds.Count; i++)
            {
                var document = await _context.DocArticles.FirstOrDefaultAsync(d => d.Id == sortedIds[i]);
                if (document != null)
                {
                    document.IndexNo = i + 1;
                }
            }

            await _context.SaveChangesAsync();

        }
    }
}
