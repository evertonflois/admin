using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class UserPreferences : BaseEntity
{
    [EntityField(IsKey = true)]
    public Guid? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? Login { get; set; }
    public string? MenuType { get; set; }
    public string? MenuColor { get; set; }    
}
