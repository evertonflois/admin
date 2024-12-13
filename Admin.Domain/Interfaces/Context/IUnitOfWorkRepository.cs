using System.Data;
using Admin.Domain.Interfaces.UoW;

namespace Admin.Domain.Interfaces.Context.Connection
{
    /// <summary>
    /// Compartilhada conexão com os repositórios
    /// </summary>
    public interface IUnitOfWorkRepository : IUnitOfWork
    {
        /// <summary>
        /// Conexão disponível
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Transação disponível
        /// </summary>
        IDbTransaction Transaction { get; }

        int _connectionTimeOut { get; set; }
    }
}

