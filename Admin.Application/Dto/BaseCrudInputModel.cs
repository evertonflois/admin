using Admin.Application.Interfaces.Dto;

namespace Admin.Application.Dto
{
    public abstract class BaseCrudInputModel : ICrudInputModel
    {
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
