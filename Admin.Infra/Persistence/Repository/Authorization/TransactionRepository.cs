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
    public class TransactionRepository : RepositoryBase<Transaction, ResponseBase>, ITransactionRepository
    {
        public TransactionRepository(IUnitOfWorkRepository uoW) : base(uoW, "Transaction")
        {
        }

        public override string getAllQueryFilterPrincipal(string sortField, string sortOrder)
        {
            return string.Format(@"select 
                                        rowNumber = row_number() over (order by {0}{1} {2}),
                                        t1.SubscriberId,
	                                    ProfileCode = isnull(t2.ProfileCode, @ProfileCode),
	                                    t1.TransactionCode,
	                                    t1.Description,
                                        t1.Active,
	                                    fl_perm = case when t2.TransactionCode is not null then 'Y' else 'N' end                                        
                                    FROM
	                                    Transaction t1 (nolock)
                                    LEFT JOIN
	                                    ProfileTransactions t2 (nolock)
                                    ON
	                                    t2.SubscriberId = t1.SubscriberId
                                    AND
	                                    t2.TransactionCode = t1.TransactionCode
                                    AND
	                                    t2.ProfileCode = @ProfileCode",
                                    getColumnPrefix(sortField),
                                    sortField,
                                    sortOrder,
                                    _table);
        }

        public override string getAllQueryFilterCondition(IEnumerable<Filter> filter)
        {
            if (filter != null)
            {
                if (filter.Any(f => f.Name == "fl_perm"))
                {
                    var sqlQueryCondition = base.getAllQueryFilterCondition(filter.Where(f => f.Name != "fl_perm" && f.Name != "ProfileCode"));

                    sqlQueryCondition = string.Concat(sqlQueryCondition, addQueryWhere(sqlQueryCondition));

                    var fl_perm = filter.First(f => f.Name == "fl_perm").Value;

                    if (fl_perm == "Y")
                        sqlQueryCondition = string.Concat(sqlQueryCondition, " and t2.TransactionCode is not null ");
                    else
                        sqlQueryCondition = string.Concat(sqlQueryCondition, " and t2.TransactionCode is null ");

                    return sqlQueryCondition;
                }
            }
            
            return base.getAllQueryFilterCondition(filter.Where(f => f.Name != "ProfileCode"));
        }

        public override string getColumnPrefix(string? column)
        {
            switch(column)
            {
                case "SubscriberId":
                case "TransactionCode":
                    return "t1.";
                default:
                    return "";
            }
        }
                
        public async Task<IEnumerable<Transaction>> GetByProfile(string ProfileCode)
        {
            var query = @"
                    SELECT 
	                    t1.*
                    FROM
	                    Transaction t1 (nolock)
                    INNER JOIN
	                    ProfileTransactions t2 (nolock)
                    ON
	                    t2.TransactionCode = t1.TransactionCode
                    WHERE
	                    t2.ProfileCode = @ProfileCode
                    AND
                        t1.Active = 'Y'
            ";

            var param = new { ProfileCode };

            return
            await _connection.QueryAsync<Transaction>(
                query,
                param,
                _transaction,
                commandType: CommandType.Text
            );
        }

        public async Task<IEnumerable<Transaction>> GetAllByProfile(string ProfileCode, string fl_perm)
        {
            var query = @"
                    SELECT 
	                    t1.*
                    FROM
	                    Transaction t1 (nolock)
                    INNER JOIN
	                    ProfileTransactions t2 (nolock)
                    ON
	                    t2.TransactionCode = t1.TransactionCode
                    WHERE
	                    t2.ProfileCode = @ProfileCode
                    AND
                        t1.Active = 'Y'
            ";

            var param = new { ProfileCode };

            return
            await _connection.QueryAsync<Transaction>(
                query,
                param,
                _transaction,
                commandType: CommandType.Text
            );
        }
    }
}
