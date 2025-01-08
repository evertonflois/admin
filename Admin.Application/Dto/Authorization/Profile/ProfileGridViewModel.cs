namespace Admin.Application.Dto.Authorization.Profile;

public class ProfileGridViewModel : BaseGridViewModel
{
    public Guid? SubscriberId { get; set; }
    public string? ProfileCode { get; set; }
    public string? Description { get; set; }
    public string? Active { get; set; }
}
