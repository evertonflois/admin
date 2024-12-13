using Admin.Application.Dto.Authorization.TransactionActions;

namespace Admin.Application.Dto.Authorization.Transaction;

public class TransactionChangeInputModel : BaseCrudInputModel
{
    public string? SubscriberId { get; set; }
    public string? ProfileCode { get; set; }
    public string? TransactionCode { get; set; }
    public string? Description { get; set; }
    public string? Active { get; set; }
    public bool? FlagPermission { get; set; }
    public bool? FlagOriginalPermission { get; set; }
    public IEnumerable<TransactionActionChangeInputModel>? Actions { get; set; }
}
