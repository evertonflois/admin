namespace Admin.Domain.Entities.Authorization;

public class Menu : BaseEntity
{
    public string? MenuId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? TransactionCode { get; set; }
    public string? ParentMenuId { get; set; }
    public short Level { get; set; }
    public short Order { get; set; }
    public string? GroupingCode { get; set; }
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public string? Active { get; set; }    
}
