using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{
    public interface ITransactionRepository : IRepositoryBase<Transaction, ResponseBase>
    {
        Task<IEnumerable<Transaction>> GetByProfile(string profileCode);
    }
}
