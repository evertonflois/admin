using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class User : BaseEntity
{
    [EntityField(IsKey = true)]
    public string? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? ProfileCode { get; set; }
    [EntityField(IgnoreOnUpdate = true)]
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Active { get; set; }
    [EntityField(IgnoreOnUpdate = true)]
    public int LoginCounter { get; set; }
    [EntityField(IgnoreOnUpdate = true)]
    public short LoginErrorCounter { get; set; }
}
