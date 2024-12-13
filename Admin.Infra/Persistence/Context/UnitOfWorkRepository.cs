using System;
using System.Data;
using System.Data.Common;

using Microsoft.Extensions.Configuration;

using Admin.Domain.Interfaces.Context.Connection;
using Admin.Domain.Interfaces.UoW;

namespace Admin.Infra.Context.Connection
{
    /// <summary>
    /// Gerenciamento de conexões e transações
    /// </summary>
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private DbConnection _connection { get; set; }
        public DbTransaction _transaction { get; set; }

        public IDbConnection Connection => _connection ?? throw new DataException("A conexão está fechada.");

        public IDbTransaction Transaction => _transaction;

        public Guid Identifier { get; } = Guid.NewGuid();

        public int _connectionTimeOut { get; set; }

        private readonly IConnectionFactory _connectionFactory;

        private readonly IConfiguration _configuration;

        public UnitOfWorkRepository(IConnectionFactory connectionFactory, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _connectionTimeOut = Convert.ToInt32(_configuration.GetSection("Parameters:CommandTimeout").Value);
        }

        public async Task<IUnitOfWork> BeginTransactionAsync(string keyElement = "Database:ConnectionString")
        {
            await OpenConnectionAsync(keyElement);
            
            if (_transaction is null)
                _transaction = await  _connection.BeginTransactionAsync();

            return this;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _transaction?.CommitAsync();
            }
            catch
            {
                await _transaction?.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_transaction is not null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection is not null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public async Task<IUnitOfWork> OpenConnectionAsync(string keyElement = "Database:ConnectionString")
        {
            if (_connection is null)
            {
                _connection = _connectionFactory.CreateConn(keyElement);
                await _connection.OpenAsync();
            }

            return this;
        }

    }
}