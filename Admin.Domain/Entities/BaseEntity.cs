using Admin.Domain.Attributes;

namespace Admin.Domain.Entities
{
    public class BaseEntity
    {
        [EntityField(IgnoreOnUpdate = true)]
        public string? CreationUser { get; set; }
        [EntityField(IgnoreOnUpdate = true)]
        public DateTime? CreationDate { get; set; }

        public string? ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        
        protected string FlagDescription(string flag)
        {
            return flag == "Y" ? "Yes" : flag == "N" ? "No" : flag;
        }

        [EntityField(Persists = false)]
        public int? Rowscount { get; set; }
    }    
}
