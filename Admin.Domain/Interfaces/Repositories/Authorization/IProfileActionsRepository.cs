using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{    
    public interface IProfileActionsRepository : IRepositoryBase<ProfileActions, ResponseBase>
    {        
        Task<ResponseBase> RemoveByTransactionAsync(object key);
    }
}
