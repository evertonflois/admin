using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{    
    public interface IMenuRepository : IRepositoryBase<Menu, ResponseBase>
    {        
        Task<IEnumerable<Menu>> GetByProfile(string profileCode);
    }
}
