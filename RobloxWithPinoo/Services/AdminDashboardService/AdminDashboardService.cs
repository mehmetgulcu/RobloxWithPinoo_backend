
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Context;
using RobloxWithPinoo.Entity.Dtos.AdminDashboardDtos;
using RobloxWithPinoo.Entity.Entities;
using System.Globalization;

namespace RobloxWithPinoo.Services.AdminDashboardService
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminDashboardService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<NumberOfArticlesPerCategory>> GetCategoryArticleCounts(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var categoryArticleCounts = new List<NumberOfArticlesPerCategory>();

            var categories = await _context.DocCategories.OrderBy(c => c.IndexNo).ToListAsync();

            foreach (var category in categories)
            {
                var articleCount = _context.DocArticles.Count(a => a.DocCategoryId == category.Id);

                categoryArticleCounts.Add(new NumberOfArticlesPerCategory
                {
                    CategoryName = category.Name,
                    ArticleCount = articleCount
                });
            }

            return categoryArticleCounts;
        }

        public async Task<DailyRegisterChart> GetDailyRegisterChart(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var today = DateTime.Today;
            var count = await _context.Users.CountAsync(u => EF.Functions.DateDiffDay(u.DateOfRegistration, today) == 0);
            var dayName = today.ToString("dddd", new CultureInfo("tr-TR"));

            return new DailyRegisterChart { Count = count, RegisterDate = today.ToString("dd/MM/yyyy"), RegisterDayName = dayName };
        }

        public async Task<GeneratedActivationCodesCount> GetGeneratedActivationCodesCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.ActivationCodes.Count();

            return new GeneratedActivationCodesCount { Count = count };
        }

        public async Task<List<MonthlyRegisterChart>> GetMonthlyRegisterChart(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var currentDate = DateTime.Today;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var monthlyData = new List<MonthlyRegisterChart>();

            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                var count = await _context.Users.CountAsync(u => EF.Functions.DateDiffDay(u.DateOfRegistration, date) == 0);
                var monthName = date.ToString("MMMM", new CultureInfo("tr-TR"));
                monthlyData.Add(new MonthlyRegisterChart
                {
                    Date = date.ToString("dd/MM/yyyy"),
                    Month = monthName,
                    Count = count
                });
            }

            return monthlyData;
        }

        public async Task<TotalActivatedCodesCount> GetTotalActivatedCodesCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.ActivationCodes.Where(a => a.IsActive == true).Count();

            return new TotalActivatedCodesCount { Count = count };
        }

        public async Task<TotalActiveCardsCount> GetTotalActiveCardCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.ActivationCodes.Where(a => a.IsActive == true).Count();

            return new TotalActiveCardsCount { Count = count };
        }

        public async Task<TotalDocArticlesCount> GetTotalDocArticlesCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.DocArticles.Count();

            return new TotalDocArticlesCount { Count = count };
        }

        public async Task<TotalDocCategoriesCount> GetTotalDocCategoriesCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.DocCategories.Count();

            return new TotalDocCategoriesCount { Count = count };
        }

        public async Task<TotalNotActivatedCodesCount> GetTotalNotActivatedCodesCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.ActivationCodes.Where(a => a.IsActive == false).Count();

            return new TotalNotActivatedCodesCount { Count = count };
        }

        public async Task<TotalUsersCount> GetTotalUsersCount(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var count = _context.Users.Count();

            return new TotalUsersCount { Count = count };
        }

        public async Task<List<WeeklyRegisterChart>> GetWeeklyRegisterChart(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek).AddDays(1);
            var endOfWeek = startOfWeek.AddDays(6);

            var weeklyData = new List<WeeklyRegisterChart>();

            for (DateTime date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                var count = await _context.Users.CountAsync(u => EF.Functions.DateDiffDay(u.DateOfRegistration, date) == 0);
                var dayName = date.ToString("dddd", new CultureInfo("tr-TR"));
                weeklyData.Add(new WeeklyRegisterChart
                {
                    Date = date.ToString("dd/MM/yyyy"),
                    DayOfWeek = dayName,
                    Count = count
                });
            }

            return weeklyData;
        }

        public async Task<List<YearlyRegisterChart>> GetYearlyRegisterChart(Guid appUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(appUserId.ToString());

            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok.");
            }

            var currentDate = DateTime.Today;
            var currentYear = currentDate.Year;

            var yearlyData = new List<YearlyRegisterChart>();

            for (int month = 1; month <= 12; month++)
            {
                var firstDayOfMonth = new DateTime(currentYear, month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var monthlyCount = await _context.Users
                    .CountAsync(u => u.DateOfRegistration >= firstDayOfMonth && u.DateOfRegistration <= lastDayOfMonth);

                var monthName = firstDayOfMonth.ToString("MMMM", new CultureInfo("tr-TR"));
                yearlyData.Add(new YearlyRegisterChart
                {
                    Month = monthName,
                    Count = monthlyCount
                });
            }

            return yearlyData;
        }

    }
}
