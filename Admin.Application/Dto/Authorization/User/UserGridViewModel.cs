namespace Admin.Application.Dto.Authorization.User;

public class UserGridViewModel : BaseGridViewModel
{
    public string? SubscriberId { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? ProfileCode { get; set; }
    public string? ProfileDescription { get; set; }
}
