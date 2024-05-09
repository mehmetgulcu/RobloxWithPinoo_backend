namespace RobloxWithPinoo.Entity.Dtos.ActivationCode
{
    public class ActivationCodeList
    {
        public Guid Code { get; set; }
        public bool IsActive { get; set; }
        public string ActivatedDate { get; set; }
        public string ActivetedUserName { get; set; }
        public string ActivetedUserLastName { get; set; }
        public string CreatedDate { get; set; }
    }
}
