using Admin.Infra.Persistence.Repository;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;
using Dapper;
using System.Data;

namespace Admin.Infra.Repositories.Authorization
{
    public class ProfileActionsRepository : RepositoryBase<ProfileActions, ResponseBase>, IProfileActionsRepository
    {
        public ProfileActionsRepository(IUnitOfWorkRepository uoW) : base(uoW, "ProfileActions")
        {
        }
    
        public async Task<ResponseBase> RemoveByTransactionAsync(object key)
        {
            string sqlRemoveCommand = @"BEGIN TRY 
                                            DELETE ProfileActions WHERE SubscriberId = @SubscriberId AND ProfileCode = @ProfileCode AND TransactionCode = @TransactionCode;
                                            SELECT 0 AS return_code,'Record has been removed.' AS return_chav
                                        END TRY
                                        BEGIN CATCH
                                            SELECT @@ERROR AS return_code, ERROR_MESSAGE() AS return_chav
                                        END CATCH";            

            return await _connection.QueryFirstAsync<ResponseBase>(
               sqlRemoveCommand,
               key,
               _transaction,
               commandType: CommandType.Text
            );
        }
    }
}
