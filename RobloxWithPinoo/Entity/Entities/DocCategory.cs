using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class DocCategory : EntityBase
    {
        public string Name { get; set; }

        public int IndexNo { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<DocArticle> Articles { get; set; }
    }
}
