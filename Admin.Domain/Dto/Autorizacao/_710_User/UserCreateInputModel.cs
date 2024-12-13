using Admin.Domain.Entities.Autorizacao;

namespace Admin.Application.Dto.Autorizacao._710_User
{
    public class UserCreateInputModel : BaseCrudCreateModel, IMapFrom<E_710_User>
    {
        public short cd_asnt { get; set; }
        public int nr_enti { get; set; }
        public string cd_user { get; set; }
        public string nm_user { get; set; }
        public string cd_grup_auto_trsc { get; set; }
        public string cd_senh { get; set; }
        public string fl_atvo { get; set; }
        public int qt_aces { get; set; }
    }
}
