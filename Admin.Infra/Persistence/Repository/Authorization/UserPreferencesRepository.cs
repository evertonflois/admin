using Admin.Infra.Persistence.Repository;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;

namespace Admin.Infra.Repositories.Authorization
{
    public class UserPreferencesRepository : RepositoryBase<UserPreferences, ResponseBase>, IUserPreferencesRepository
    {
        public UserPreferencesRepository(IUnitOfWorkRepository uoW) : base(uoW, "UserPreferencesRepository")
        {
        }        
    }
}
