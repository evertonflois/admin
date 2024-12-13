using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{    
    public interface IUserRepository : IRepositoryBase<User, ResponseBase>
    {        
    }
}
