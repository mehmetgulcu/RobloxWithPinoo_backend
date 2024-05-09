namespace RobloxWithPinoo.Entity.Dtos.DocArticle
{
    public class UpdateDocArticle
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public Guid DocCategoryId { get; set; }
    }
}
