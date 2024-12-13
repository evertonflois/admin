using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{
    public interface ISubscriberRepository : IRepositoryBase<Subscriber, ResponseBase>
    {
        Task<IEnumerable<Subscriber>> GetComboAsync();
    }
}
