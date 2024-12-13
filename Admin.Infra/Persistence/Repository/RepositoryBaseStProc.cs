using System.Data;
using Dapper;
using Admin.Domain.Interfaces.Repositories;
using Admin.Domain.Entities;
using Admin.Domain.Attributes;
using Admin.Domain.Interfaces.Context.Connection;

namespace Admin.Infra.Persistence.Repository
{
    /// <summary>
    /// Classe para obter conexão para executar os comandos.
    /// </summary>
    public class RepositoryBaseStProc<TEntity> : IRepositoryBaseStProc<TEntity> where TEntity : class
    {
        private readonly IUnitOfWorkRepository _uoW;

        /// <summary>
        /// Conexão para executar comandos e consultas
        /// </summary>
        protected IDbConnection _connection => _uoW.Connection;

        /// <summary>
        /// Transação para executar comandos e consultas
        /// </summary>
        protected IDbTransaction _transaction => _uoW.Transaction;

        public string _stProcS = string.Empty;
        public string _stProcM = string.Empty;

        public RepositoryBaseStProc(IUnitOfWorkRepository uoW)
        {
            _uoW = uoW;
            _stProcS = GetStProcQuery(typeof(TEntity));
            _stProcM = GetStProcMaintenance(typeof(TEntity));
            //SqlMapper.Settings.CommandTimeout = _uoW._connectionTimeOut;
        }

        /// <summary>
        /// Metodo base para consulta de seleção do grid.
        /// </summary>
        public async Task<IEnumerable<TEntity>> FindAsync(object param)
        {
            return
                await _connection.QueryAsync<TEntity>(
                    _stProcS,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para consulta de seleção para combo ou consulta sem paginação
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAllAsync(object param)
        {
            return
                await _connection.QueryAsync<TEntity>(
                    _stProcS,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para consulta de seleção para edição
        /// </summary>
        public async Task<TEntity?> GetByIdAsync(object param)
        {
            return
                await _connection.QueryFirstOrDefaultAsync<TEntity>(
                    _stProcS,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para consulta de seleção para edição
        /// </summary>
        public async Task<TEntity?> GetByTextAsync(object param)
        {
            return
                await _connection.QueryFirstOrDefaultAsync<TEntity>(
                    _stProcS,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para consulta de seleção para edição
        /// </summary>
        public async Task<TEntity?> GetItemByChaveAsync(object param)
        {
            return
                await _connection.QueryFirstOrDefaultAsync<TEntity>(
                    _stProcS,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para criação de novo registro
        /// </summary>
        public async Task<ResponseBase> CreateAsync(object param)
        {
            return
                await _connection.QueryFirstAsync<ResponseBase>(
                    _stProcM,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para alteração de um registro
        /// </summary>
        public async Task<ResponseBase> UpdateAsync(object param)
        {
            return
                await _connection.QueryFirstAsync<ResponseBase>(
                    _stProcM,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo base para exclusão de um registro
        /// </summary>
        public async Task<ResponseBase> RemoveAsync(object param)
        {
            return await _connection.QueryFirstAsync<ResponseBase>(
               _stProcM,
               param,
               _transaction,
               commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Metodo base para execução de comando de manutenção
        /// </summary>
        public async Task<ResponseBase> MaintenanceCommandAsync(object param, string stProc = "")
        {
            return
                await _connection.QueryFirstAsync<ResponseBase>(
                    !string.IsNullOrEmpty(stProc) ? stProc : _stProcM,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Metodo para obter o nome da proc de manutenção da entidade 
        /// </summary>
        private string GetStProcMaintenance(Type entityType)
        {
            return entityType.GetAttributeValue((ProcedureMaintenanceAttribute pm) => pm.stProc);
        }

        /// <summary>
        /// Metodo para obter o nome da proc de seleção da entidade 
        /// </summary>
        private string GetStProcQuery(Type entityType)
        {
            return entityType.GetAttributeValue((ProcedureQueryAttribute pm) => pm.stProc);
        }
    }
}