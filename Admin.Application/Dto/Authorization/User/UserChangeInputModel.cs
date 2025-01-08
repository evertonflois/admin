namespace Admin.Application.Dto.Authorization.User;

public class UserChangeInputModel : BaseCrudInputModel
{
    public Guid? SubscriberId { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? ProfileCode { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Active { get; set; }
    public int LoginCounter { get; set; }
}
