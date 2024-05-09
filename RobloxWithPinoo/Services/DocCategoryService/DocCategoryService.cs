using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Dtos.DocCategory;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.DocCategoryService
{
    public class DocCategoryService : IDocCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DocCategoryService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> CreateDocCategoryAsync(CreateDocCategoryDto createDocCategoryDto, Guid appUserId)
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

            var existingCategory = await _context.DocCategories
                .FirstOrDefaultAsync(c => c.Name == createDocCategoryDto.Name && c.AppUserId == appUserId);

            if (existingCategory != null)
            {
                throw new Exception("Böyle bir kategori zaten var.");
            }

            var newCategory = new DocCategory
            {
                Name = createDocCategoryDto.Name,
                AppUserId = appUserId,
                CreatedDate = DateTime.Now,
                CreatedBy = currentUser.UserName,
                IsDeleted = false,
            };

            _context.DocCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDocCategoryAsync(Guid categoryId, Guid appUserId)
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

            var category = await _context.DocCategories.FirstOrDefaultAsync(x => x.Id == categoryId);

            _context.DocCategories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ListDocCategories>> GetAllDocCategoryForAllUser(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                var allDocCategories = await _context.DocCategories.OrderBy(x => x.IndexNo).ToListAsync();

                if (!allDocCategories.Any())
                {
                    throw new KeyNotFoundException("Makale bulunamadı.");
                }

                var categoryDtos = allDocCategories.Select(category => new ListDocCategories
                {
                    Id = category.Id,
                    Name = category.Name,
                    IndexNo = category.IndexNo,
                    CreatedDate = category.CreatedDate.ToString("dd/MM/yyyy"),
                }).ToList();

                return categoryDtos;
            }
            else
            {
                var activeActivationCodes = await _context.ActivationCodes.Where(x => x.ActivatedUserId == currentUser.Id && x.IsActive).ToListAsync();


                if (!activeActivationCodes.Any())
                {
                    return new List<ListDocCategories>();
                }

                var allDocCategories = await _context.DocCategories.OrderBy(x => x.IndexNo).ToListAsync();

                if (!allDocCategories.Any())
                {
                    throw new KeyNotFoundException("Makale bulunamadı.");
                }

                var categoryDtos = allDocCategories.Select(category => new ListDocCategories
                {
                    Id = category.Id,
                    Name = category.Name,
                    IndexNo = category.IndexNo,
                    CreatedDate = category.CreatedDate.ToString("dd/MM/yyyy"),
                }).ToList();

                return categoryDtos;
            }


        }


        public async Task<DocCategoryDto> GetDocCategoryByIdAsync(Guid categoryId, Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            var category = await _context.DocCategories
                .FirstOrDefaultAsync(a => a.Id == categoryId);

            if (category == null)
            {
                throw new KeyNotFoundException("Kategori bulunamadı.");
            }

            var categoryDto = new DocCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate.ToString("dd/MM/yyyy"),
                IndexNo = category.IndexNo
            };

            return categoryDto;
        }

        public async Task<bool> UpdateDocCategoryAsync(UpdateDocCategoryDto updateDocCategoryDto, Guid categoryId, Guid appUserId)
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

            var category = await _context.DocCategories
                .FirstOrDefaultAsync(a => a.Id == categoryId);

            if (category != null)
            {

                category.Id = categoryId;
                category.Name = updateDocCategoryDto.Name;
                category.IndexNo = category.IndexNo;
                category.AppUserId = appUserId;
                category.ModifiedBy = currentUser.UserName;
                category.ModifiedDate = DateTime.Now;
                category.IsDeleted = false;

                _context.DocCategories.Update(category);
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
                var document = await _context.DocCategories.FirstOrDefaultAsync(d => d.Id == sortedIds[i]);
                if (document != null)
                {
                    document.IndexNo = i + 1;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
