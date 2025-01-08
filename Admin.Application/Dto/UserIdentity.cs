namespace Admin.Application.Dto
{
    public class UserIdentity
    {
        public Guid? SubscriberId { get; set; }
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? ProfileCode { get; set; }
    }
}
