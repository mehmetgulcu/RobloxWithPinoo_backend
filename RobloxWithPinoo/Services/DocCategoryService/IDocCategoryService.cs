using RobloxWithPinoo.Entity.Dtos.DocArticle;
using RobloxWithPinoo.Entity.Dtos.DocCategory;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Services.DocCategoryService
{
    public interface IDocCategoryService
    {
        Task<bool> CreateDocCategoryAsync(CreateDocCategoryDto createDocCategoryDto, Guid appUserId);
        Task<List<ListDocCategories>> GetAllDocCategoryForAllUser(Guid appUserId);
        Task UpdateIndexesAsync(List<Guid> sortedIds, Guid appUserId);
        Task<DocCategoryDto> GetDocCategoryByIdAsync(Guid categoryId, Guid appUserId);
        Task<bool> UpdateDocCategoryAsync(UpdateDocCategoryDto updateDocCategoryDto, Guid categoryId, Guid appUserId);
        Task<bool> DeleteDocCategoryAsync(Guid categoryId, Guid appUserId);
    }
}
