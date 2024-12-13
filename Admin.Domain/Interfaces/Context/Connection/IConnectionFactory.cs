using System.Data.Common;

namespace Admin.Domain.Interfaces.Context.Connection
{
    /// <summary>
    /// <see cref="DbConnection"/> Fábrica de DbConnection
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Cria uma nova instância de <see cref="DbConnection"/>
        /// </summary>
        DbConnection CreateConn(string keyConnectionString);
    }
}