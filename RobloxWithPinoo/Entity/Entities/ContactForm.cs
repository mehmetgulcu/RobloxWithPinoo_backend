using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class ContactForm : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
