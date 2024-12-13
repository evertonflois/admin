using AutoMapper;

using Admin.Application.Helpers;
using Admin.Application.Dto;
using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Application.Interfaces.Services.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Microsoft.Extensions.Options;
using Admin.Application.Dto.Authorization.UserPreferences;

namespace Admin.Application.Services.Authorization
{
    public class UserPreferencesAppService : CrudBaseAppService<UserPreferences, ResponseBase, UserPreferenceDetailViewModel, object, UserPreferenceCreateInputModel, UserPreferenceChangeInputModel, MaintenanceResultViewModel>, IUserPreferencesAppService
    {        
        public UserPreferencesAppService(IUnitOfWorkRepository uoW, IUserPreferencesRepository repository, IMapper mapper, IOptions<AppSettings> appSettings) : base(uoW, repository, mapper)
        {            
        }        
    }
}
