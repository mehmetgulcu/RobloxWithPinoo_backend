using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class ActivationCode : EntityBase
    {
        public Guid Code { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? ActivatedDate { get; set; }
        public string? ActivetedUserName { get; set; }
        public string? ActivetedUserLastName { get; set; }
        public Guid? ActivatedUserId { get; set; }
    }
}
