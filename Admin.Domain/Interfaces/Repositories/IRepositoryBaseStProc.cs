using Admin.Domain.Entities;

namespace Admin.Domain.Interfaces.Repositories
{
    public interface IRepositoryBaseStProc<TEntity>
    {
        Task<IEnumerable<TEntity>> FindAsync(object param);
        Task<IEnumerable<TEntity>> GetAllAsync(object param);
        Task<TEntity?> GetByIdAsync(object param);
        Task<TEntity?> GetByTextAsync(object param);
        Task<TEntity?> GetItemByChaveAsync(object param);
        Task<ResponseBase> CreateAsync(object param);
        Task<ResponseBase> UpdateAsync(object param);
        Task<ResponseBase> RemoveAsync(object param);
        Task<ResponseBase> MaintenanceCommandAsync(object param, string stProc = "");
    }
}
