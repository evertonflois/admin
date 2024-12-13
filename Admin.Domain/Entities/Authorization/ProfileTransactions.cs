using Admin.Domain.Attributes;

namespace Admin.Domain.Entities.Authorization;

public class ProfileTransactions : BaseEntity
{
    [EntityField(IsKey = true)]
    public string? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? ProfileCode { get; set; }
    [EntityField(IsKey = true)]
    public string? TransactionCode { get; set; }    
}
