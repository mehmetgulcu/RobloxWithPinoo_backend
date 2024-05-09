using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class DocArticle : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public int IndexNo { get; set; }

        public Guid DocCategoryId { get; set; }
        public DocCategory DocCategory { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
