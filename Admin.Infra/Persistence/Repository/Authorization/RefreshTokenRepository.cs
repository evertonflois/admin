using Admin.Infra.Persistence.Repository;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;
using Dapper;
using System.Data;

namespace Admin.Infra.Repositories.Authorization
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken, ResponseBase>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IUnitOfWorkRepository uoW) : base(uoW, "RefreshToken")
        {
        }
                
        public async Task<ResponseBase> RemoveOldByUserAsync(string login, int refreshTokenTTL)
        {

            string sqlRemoveCommand = @"BEGIN TRY 
                                            DELETE 
                                                    RefreshToken 
                                            WHERE 
                                                    Login = @login
                                            AND
                                                    (Revoked IS NOT NULL OR Expires < GETDATE())
                                            AND
                                                    DATEADD(DAY, @refreshTokenTTL, Created) <= GETDATE()
                                            ;
                                            SELECT 0 AS return_code,'Record has been removed.' AS return_chav
                                        END TRY
                                        BEGIN CATCH
                                            SELECT @@ERROR AS return_code, ERROR_MESSAGE() AS return_chav
                                        END CATCH";

            return
            await _connection.QueryFirstAsync<ResponseBase>(
                sqlRemoveCommand,
                new
                {
                    login
                    ,
                    refreshTokenTTL
                },
                _transaction,
                commandType: CommandType.Text
            );
        }
    }
}
