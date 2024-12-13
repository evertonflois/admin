namespace Admin.Application.Dto.Authorization.UserPreferences;

public class UserPreferenceCreateInputModel : BaseCrudInputModel
{    
    public string? SubscriberId { get; set; }
    public string? Login { get; set; }
    public string? MenuType { get; set; }
    public string? MenuColor { get; set; }    
}
