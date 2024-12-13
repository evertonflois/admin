using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class Profile : BaseEntity
{
    [EntityField(IsKey = true)]
    public string? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? ProfileCode { get; set; }
    public string? Description { get; set; }
    public string? Active { get; set; }    
}
