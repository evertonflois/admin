using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{    
    public interface IUserPreferencesRepository : IRepositoryBase<UserPreferences, ResponseBase>
    {        
    }
}
