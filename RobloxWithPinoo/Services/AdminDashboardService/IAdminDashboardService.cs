using RobloxWithPinoo.Entity.Dtos.AdminDashboardDtos;

namespace RobloxWithPinoo.Services.AdminDashboardService
{
    public interface IAdminDashboardService
    {
        Task<TotalDocArticlesCount> GetTotalDocArticlesCount(Guid appUserId);
        Task<TotalDocCategoriesCount> GetTotalDocCategoriesCount(Guid appUserId);
        Task<List<NumberOfArticlesPerCategory>> GetCategoryArticleCounts(Guid appUserId);
        Task<TotalActiveCardsCount> GetTotalActiveCardCount(Guid appUserId);
        Task<TotalUsersCount> GetTotalUsersCount(Guid appUserId);
        Task<DailyRegisterChart> GetDailyRegisterChart(Guid appUserId);
        Task<List<WeeklyRegisterChart>> GetWeeklyRegisterChart(Guid appUserId);
        Task<List<MonthlyRegisterChart>> GetMonthlyRegisterChart(Guid appUserId);
        Task<List<YearlyRegisterChart>> GetYearlyRegisterChart(Guid appUserId);
        Task<GeneratedActivationCodesCount> GetGeneratedActivationCodesCount(Guid appUserId);
        Task<TotalActivatedCodesCount> GetTotalActivatedCodesCount(Guid appUserId);
        Task<TotalNotActivatedCodesCount> GetTotalNotActivatedCodesCount(Guid appUserId);
    }
}
