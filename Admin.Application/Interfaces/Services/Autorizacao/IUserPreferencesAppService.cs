using Admin.Application.Dto;
using Admin.Application.Dto.Authorization.UserPreferences;

namespace Admin.Application.Interfaces.Services.Authorization
{
    public interface IUserPreferencesAppService : ICrudBaseAppService<UserPreferenceDetailViewModel, object, UserPreferenceCreateInputModel, UserPreferenceChangeInputModel, MaintenanceResultViewModel>
    {        
    }
}
