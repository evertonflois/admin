using System.Data.Common;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Admin.Domain.Interfaces.Context.Connection;

namespace Admin.Infra.Context.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        /// <summary>
        /// Acesso a string de conexão
        /// </summary>
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection CreateConn(string keyConnectionString) => new SqlConnection(
            GetConnectionString(keyConnectionString)
        );

        private string GetConnectionString(string keyElement)
        {
            return _configuration.GetSection(keyElement).Value;
        }
    }
}