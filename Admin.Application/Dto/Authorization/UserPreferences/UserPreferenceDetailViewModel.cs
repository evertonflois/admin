namespace Admin.Application.Dto.Authorization.UserPreferences;

public class UserPreferenceDetailViewModel
{
    public Guid? SubscriberId { get; set; }    
    public string? Login { get; set; }
    public string? MenuType { get; set; }
    public string? MenuColor { get; set; }
}
