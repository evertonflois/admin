namespace Admin.Domain.Entities.Authorization;

public class TransactionActions : BaseEntity
{
    public string? SubscriberId { get; set; }
    public string? TransactionCode { get; set; }
    public string? ActionCode { get; set; }
    public string? Description { get; set; }
    public string? FlagPermission { get; set; }
    public string? FlagOriginalPermission { get; set; }
    public string? Active { get; set; }    
}
