using System.Data;
using Dapper;
using Admin.Domain.Interfaces.Repositories;
using Admin.Domain.Entities;
using Admin.Domain.Attributes;
using Admin.Domain.Interfaces.Context.Connection;

namespace Admin.Infra.Persistence.Repository
{
    /// <summary>
    /// Classe para obter conex�o para executar os comandos.
    /// </summary>
    public class RepositoryBaseStProc<TEntity> : IRepositoryBaseStProc<TEntity> where TEntity : class
    {
        private readonly IUnitOfWorkRepository _uoW;

        /// <summary>
        /// Conex�o para executar comandos e consultas
        /// </summary>
        protected IDbConnection _connection => _uoW.Connection;

        /// <summary>
        /// Transa��o para executar comandos e consultas
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
        /// Metodo base para consulta de sele��o do grid.
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
        /// Metodo base para consulta de sele��o para combo ou consulta sem pagina��o
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
        /// Metodo base para consulta de sele��o para edi��o
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
        /// Metodo base para consulta de sele��o para edi��o
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
        /// Metodo base para consulta de sele��o para edi��o
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
        /// Metodo base para cria��o de novo registro
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
        /// Metodo base para altera��o de um registro
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
        /// Metodo base para exclus�o de um registro
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
        /// Metodo base para execu��o de comando de manuten��o
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
        /// Metodo para obter o nome da proc de manuten��o da entidade 
        /// </summary>
        private string GetStProcMaintenance(Type entityType)
        {
            return entityType.GetAttributeValue((ProcedureMaintenanceAttribute pm) => pm.stProc);
        }

        /// <summary>
        /// Metodo para obter o nome da proc de sele��o da entidade 
        /// </summary>
        private string GetStProcQuery(Type entityType)
        {
            return entityType.GetAttributeValue((ProcedureQueryAttribute pm) => pm.stProc);
        }
    }
}