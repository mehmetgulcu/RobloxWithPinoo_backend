namespace RobloxWithPinoo.Entity.Dtos.ContactFormDtos
{
    public class AllContactForms
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }
}
