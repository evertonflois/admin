namespace Admin.Application.Dto
{
    public abstract class BaseCrudCreateModel
    {
        public string? cd_user_cadm { get; set; }
        public DateTime? ts_cadm { get; set; }
        public string? cd_user_manu { get; set; }
        public DateTime? ts_manu { get; set; }
    }
}
