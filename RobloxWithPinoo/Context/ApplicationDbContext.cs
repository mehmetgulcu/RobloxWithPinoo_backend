using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<DocCategory> DocCategories { get; set; }
        public DbSet<DocArticle> DocArticles { get; set; }
        public DbSet<ActivationCode> ActivationCodes { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .HasOne(p => p.AppUser)
                .WithMany(t => t.CardControls)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocArticle>()
                .HasOne(a => a.DocCategory)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.DocCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
