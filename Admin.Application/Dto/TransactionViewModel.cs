namespace Admin.Application.Dto
{
    public class TransactionViewModel
    {
        public string Code { get; set; }
        public string? Description { get; set; }
        public string[]? Permissions { get; set; }
    }
}
