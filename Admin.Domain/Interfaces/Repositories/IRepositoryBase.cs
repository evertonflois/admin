using Admin.Domain.Entities;

namespace Admin.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity, TMaitenanceResult>
    {        
        Task<IEnumerable<TEntity>> GetAllAsync(object param);
        Task<IEnumerable<TEntity>> GetAllFilterAsync(IEnumerable<Filter> filter, string sortField, string sortOrder, int pageNumber, int pageSize);
        Task<TEntity?> GetByIdAsync(object param);
        Task<TMaitenanceResult> CreateAsync(object param);
        Task<TMaitenanceResult> ChangeAsync(object param);
        Task<TMaitenanceResult> RemoveAsync(object key);
        Task<TMaitenanceResult> CommandAsync(object param, string command);
        Task<TMaitenanceResult> CommandStProcAsync(object param, string stProc);
    }
}
