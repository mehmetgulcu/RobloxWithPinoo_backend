using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RobloxWithPinoo.Entity.BaseEntity;

namespace RobloxWithPinoo.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public AppUser()
        {
            CardControls = new List<Card>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfRegistration { get; set; }


        [JsonIgnore]
        public ICollection<Card> CardControls { get; set; }
    }
}
