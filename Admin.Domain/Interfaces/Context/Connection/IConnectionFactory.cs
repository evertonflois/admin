using System.Data.Common;

namespace Admin.Domain.Interfaces.Context.Connection
{
    /// <summary>
    /// <see cref="DbConnection"/> F�brica de DbConnection
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Cria uma nova inst�ncia de <see cref="DbConnection"/>
        /// </summary>
        DbConnection CreateConn(string keyConnectionString);
    }
}