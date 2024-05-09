using RobloxWithPinoo.Entity.Dtos.DocArticle;

namespace RobloxWithPinoo.Services.DocArticleService
{
    public interface IDocArticleService
    {
        Task<bool> CreateDocArticleAsync(CreateDocArticleDto createDocArticleDto, Guid appUserId);
        Task<DocArticleDto> GetDocArticleByIdAsync(Guid articleId, Guid appUserId);
        Task<List<DocArticleDto>> GetDocArticleByCategoryAsync(Guid categoryId, Guid appUserId);
        Task<List<ListDocArticles>> GetAllDocArticles(Guid appUserId);
        Task UpdateIndexesAsync(List<Guid> sortedIds, Guid appUserId);
        Task<bool> UpdateDocArticleAsync(UpdateDocArticle updateDocArticle, Guid articleId, Guid appUserId);
        Task<bool> DeleteDocArticleAsync(Guid articleId, Guid appUserId);
    }
}
