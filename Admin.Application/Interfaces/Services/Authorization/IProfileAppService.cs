using Admin.Application.Dto;
using Admin.Application.Dto.Authorization.Profile;

namespace Admin.Application.Interfaces.Services.Authorization
{
    public interface IProfileAppService : ICrudBaseAppService<ProfileDetailViewModel, ProfileGridViewModel, ProfileCreateInputModel, ProfileChangeInputModel, MaintenanceResultViewModel>
    {
        Task<IEnumerable<ProfileComboViewModel>> GetCombo(string subscriberId);
    }
}
