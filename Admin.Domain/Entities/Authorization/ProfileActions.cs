using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class ProfileActions : BaseEntity
{
    [EntityField(IsKey = true)]
    public Guid? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? ProfileCode { get; set; }
    [EntityField(IsKey = true)]
    public string? TransactionCode { get; set; }
    [EntityField(IsKey = true)]
    public string? ActionCode { get; set; }
}
