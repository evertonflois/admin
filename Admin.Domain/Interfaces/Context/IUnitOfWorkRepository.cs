using System.Data;
using Admin.Domain.Interfaces.UoW;

namespace Admin.Domain.Interfaces.Context.Connection
{
    /// <summary>
    /// Compartilhada conex�o com os reposit�rios
    /// </summary>
    public interface IUnitOfWorkRepository : IUnitOfWork
    {
        /// <summary>
        /// Conex�o dispon�vel
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Transa��o dispon�vel
        /// </summary>
        IDbTransaction Transaction { get; }

        int _connectionTimeOut { get; set; }
    }
}

