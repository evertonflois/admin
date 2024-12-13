using Admin.Infra.Persistence.Repository;
using Admin.Domain.Entities.Authorization;
using Admin.Domain.Interfaces.Repositories.Authorization;
using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Entities;

namespace Admin.Infra.Repositories.Authorization
{
    public class UserRepository : RepositoryBase<User, ResponseBase>, IUserRepository
    {
        public UserRepository(IUnitOfWorkRepository uoW) : base(uoW, "User")
        {
        }

        public override string getAllQueryFilterPrincipal(string sortField, string sortOrder)
        {
            return string.Format(@"select 
                                    rowNumber = row_number() over (order by {0}{1} {2}),
                                    t1.*,
                                    t2.Description
                                from 
                                    User t1 (nolock)
                                left join
                                    Profile t2 (nolock)
                                on
                                    t2.ProfileCode = t1.ProfileCode
                                ",
                                    getColumnPrefix(sortField),
                                    sortField,
                                    sortOrder);
        }

        public override string getColumnPrefix(string? column)
        {
            if (column != null)
            {
                if (column.Equals("Description"))
                    return "t2.";
                else if (column.Equals("Active"))
                    return "t1.";
            }

            return "";
        }
    }
}
