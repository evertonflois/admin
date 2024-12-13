using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class Transaction : BaseEntity
{
    [EntityField(IsKey = true)]
    public string? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? TransactionCode { get; set; }
    public string? Description { get; set; }
    public string? FlagPermission { get; set; }
    public string? FlagOriginalPermission { get; set; }
    public string? Active { get; set; }
}
