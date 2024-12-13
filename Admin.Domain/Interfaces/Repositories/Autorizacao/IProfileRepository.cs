using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{    
    public interface IProfileRepository : IRepositoryBase<Profile, ResponseBase>
    {        
        Task<IEnumerable<Profile>> GetComboAsync(string SubscriberId);
    }
}
