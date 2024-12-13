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
    public class ProfileRepository : RepositoryBase<Profile, ResponseBase>, IProfileRepository
    {
        public ProfileRepository(IUnitOfWorkRepository uoW) : base(uoW, "Profile")
        {
        }
        
        public async Task<IEnumerable<Profile>> GetComboAsync(string subscriberId)
        {
            var query = @"
                    SELECT 
	                     ProfileCode
                        ,Description                    
                    FROM
	                    Profile (nolock)                    
                    WHERE	                    
                        Active = 'Y'
                    AND
                        SubscriberId = @SubscriberId
            ";

            var param = new { SubscriberId = subscriberId };

            return
            await _connection.QueryAsync<Profile>(
                query,
                param,
                _transaction,
                commandType: CommandType.Text
            );
        }        
    }
}
