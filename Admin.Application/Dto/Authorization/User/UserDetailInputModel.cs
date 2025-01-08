namespace Admin.Application.Dto.Authorization.User;

public class UserDetailInputModel
{
    public Guid? SubscriberId { get; set; }    
    public string? Login { get; set; }
}
