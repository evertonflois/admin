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
    public class TransactionActionsRepository : RepositoryBase<TransactionActions, ResponseBase>, ITransactionActionsRepository
    {
        public TransactionActionsRepository(IUnitOfWorkRepository uoW) : base(uoW, "TransactionActions")
        {
        }

        public override string getAllQueryFilterPrincipal(string sortField, string sortOrder)
        {
            return string.Format(@"SELECT 
                                        rowNumber = row_number() over (order by {0}{1} {2}),
                                        t1.ActionCode,
	                                    t1.Description,
	                                    fl_perm = case when t2.ActionCode is not null then 'Y' else 'N' end
                                    FROM
	                                    TransactionActions t1 (nolock)
                                    LEFT JOIN
	                                    ProfileActions t2 (nolock)
                                    ON
	                                    t2.SubscriberId = t1.SubscriberId
                                    AND
	                                    t2.TransactionCode = t1.TransactionCode
                                    AND
	                                    t2.ActionCode = t1.ActionCode
                                    AND
	                                    t2.ProfileCode = @ProfileCode
                                    AND
                                        t2.SubscriberId = @SubscriberId
                                    ",
                                    getColumnPrefix(sortField),
                                    sortField,
                                    sortOrder,
                                    _table);
        }

        public override string getAllQueryFilterCondition(IEnumerable<Filter> filter)
        {
            return base.getAllQueryFilterCondition(filter.Where(f => f.Name != "ProfileCode" && f.Name != "SubscriberId"));
        }

        public override string getColumnPrefix(string? column)
        {
            switch (column)
            {
                case "TransactionCode":
                case "ActionCode":
                    return "t1.";
                case "SubscriberId":
                    return "t2.";
                default:
                    return "";
            }
        }
                
        public async Task<IEnumerable<TransactionActions>> GetByProfile(string profileCode)
        {
            var query = @"
                    SELECT 
	                    t1.*
                    FROM
	                    TransactionActions t1 (nolock)
                    INNER JOIN
	                    ProfileActions t2 (nolock)
                    ON
	                    t2.SubscriberId = t1.SubscriberId
                    AND
	                    t2.TransactionCode = t1.TransactionCode
                    AND
	                    t2.ActionCode = t1.ActionCode
                    WHERE
	                    t2.ProfileCode = @ProfileCode
                    AND
                        t1.Active = 'Y'
            ";

            var param = new { profileCode };

            return
            await _connection.QueryAsync<TransactionActions>(
                query,
                param,
                _transaction,
                commandType: CommandType.Text
            );
        }        
    }
}
