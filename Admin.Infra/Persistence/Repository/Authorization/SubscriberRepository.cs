using Admin.Infra.Persistence.Repository;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;
using Dapper;
using static Dapper.SqlMapper;
using System.Data;

namespace Admin.Infra.Repositories.Authorization
{    
    public class SubscriberRepository : RepositoryBase<Subscriber, ResponseBase>, ISubscriberRepository
    {
        public SubscriberRepository(IUnitOfWorkRepository uoW) : base(uoW, "Subscriber")
        {
        }
                
        public async Task<IEnumerable<Subscriber>> GetComboAsync()
        {
            var query = @"
                    SELECT 
	                     SubscriberId
                        ,Name                    
                    FROM
	                    Subscriber (nolock)                    
                    WHERE	                    
                        Active = 'Y'
            ";

            return
            await _connection.QueryAsync<Subscriber>(
                query,
                null,
                _transaction,
                commandType: CommandType.Text
            );
        }        
    }
}
