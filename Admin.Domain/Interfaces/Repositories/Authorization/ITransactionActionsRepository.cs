using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{
    public interface ITransactionActionsRepository : IRepositoryBase<TransactionActions, ResponseBase>
    {
        Task<IEnumerable<TransactionActions>> GetByProfile(string profileCode);
    }
}
