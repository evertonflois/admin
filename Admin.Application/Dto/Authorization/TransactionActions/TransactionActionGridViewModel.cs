namespace Admin.Application.Dto.Authorization.TransactionActions;

public class TransactionActionGridViewModel
{
    public string? SubscriberId { get; set; }
    public string? ProfileCode { get; set; }
    public string? TransactionCode { get; set; }
    public string? ActionCode { get; set; }
    public string? Description { get; set; }
    public bool? FlagPermission { get; set; }
    public bool? FlagOriginalPermission { get; set; }
}
