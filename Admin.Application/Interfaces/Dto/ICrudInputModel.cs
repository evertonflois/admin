namespace Admin.Application.Interfaces.Dto
{
    public interface ICrudInputModel
    {
        string? CreationUser { get; set; }
        DateTime? CreationDate { get; set; }
        string? ChangeUser { get; set; }
        DateTime? ChangeDate { get; set; }
    }
}
