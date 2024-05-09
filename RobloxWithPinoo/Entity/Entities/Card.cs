using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class Card : EntityBase
    {
        public string CardName { get; set; }

        public int? Pinoo1 { get; set; }
        public int? Pinoo2 { get; set; }
        public double? Pinoo3 { get; set; }
        public double? Pinoo4 { get; set; }
        public int? Pinoo5 { get; set; }
        public int? Pinoo6 { get; set; }
        public int? Pinoo7 { get; set; }
        public int? Pinoo8 { get; set; }
        public int? Pinoo9 { get; set; }
        public string? Pinoo10 { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
