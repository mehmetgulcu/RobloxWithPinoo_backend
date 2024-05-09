namespace RobloxWithPinoo.Entity.Dtos.DocArticle
{
    public class CreateDocArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public Guid DocCategoryId { get; set; }
    }
}
