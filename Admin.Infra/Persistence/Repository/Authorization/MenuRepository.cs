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
    public class MenuRepository : RepositoryBase<Menu, ResponseBase>, IMenuRepository
    {
        public MenuRepository(IUnitOfWorkRepository uoW) : base(uoW, "Menu")
        {
        }
		        
        public async Task<IEnumerable<Menu>> GetByProfile(string profileCode)
        {
            var query = @"
                    WITH tbl_tran as
					(
						SELECT 
							t1.*
						FROM
							Menu t1 (nolock)
						INNER JOIN
							ProfileTransactions t2 (nolock)
						ON
							t2.TransactionCode = t1.TransactionCode
						WHERE
							t2.ProfileCode = @ProfileCode
						AND
							t1.Active = 'Y'
						and
							t1.Type = 'T'
					)
					SELECT
						*
					FROM
						Menu tm (nolock)
					WHERE
						EXISTS(
						SELECT 
							top 1 1
						FROM
							tbl_tran t1 (nolock)
						WHERE
							tm.Level < t1.Level
						AND
							LEFT(tm.GroupingCode, 3) = LEFT(t1.GroupingCode, 3)
						)
					UNION ALL
					SELECT
						*
					FROM
						tbl_tran
					ORDER BY GroupingCode
            ";

            var param = new { profileCode };

            return
            await _connection.QueryAsync<Menu>(
                query,
                param,
                _transaction,
                commandType: CommandType.Text
            );
        }        
    }
}
