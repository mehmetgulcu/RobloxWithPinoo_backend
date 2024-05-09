using RobloxWithPinoo.Entity.Dtos.DocCategory;
using RobloxWithPinoo.Entity.Entities;

namespace RobloxWithPinoo.Entity.Dtos.DocArticle
{
    public class ListDocArticles
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedDate { get; set; }
        public string DocCategoryName { get; set; }
        public int IndexNo { get; set; }
    }
}
