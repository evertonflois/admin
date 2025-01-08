using Admin.Domain.Entities;
using Admin.Domain.Entities.Authorization;

namespace Admin.Domain.Interfaces.Repositories.Authorization
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, ResponseBase>
    {
        Task<ResponseBase> RemoveOldByUserAsync(string login, int refreshTokenTTL);
    }
}
