using System.Data;
using Dapper;
using Admin.Domain.Interfaces.Repositories;
using Admin.Domain.Entities;
using Admin.Domain.Attributes;
using Admin.Domain.Interfaces.Context.Connection;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Dynamic;

namespace Admin.Infra.Persistence.Repository
{
    /// <summary>
    /// Classe para obter conexão para executar os comandos.
    /// </summary>
    public class RepositoryBase<TEntity, TMaitenanceResult> : IRepositoryBase<TEntity, TMaitenanceResult> where TEntity : class
    {
        protected readonly IUnitOfWorkRepository _uoW;

        /// <summary>
        /// Conexão para executar comandos e consultas
        /// </summary>
        protected IDbConnection _connection => _uoW.Connection;

        /// <summary>
        /// Transação para executar comandos e consultas
        /// </summary>
        protected IDbTransaction _transaction => _uoW.Transaction;

        protected string _table = string.Empty;
        protected bool _flagAtivo = false;        

        public RepositoryBase(IUnitOfWorkRepository uoW, string table, bool flagAtivo = false)
        {
            _uoW = uoW;
            _flagAtivo = flagAtivo;
            _table = table;
        }        

        /// <summary>
        /// Metodo base para consulta de todos os registros
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAllAsync(object param)
        {
            return
            await _connection.QueryAsync<TEntity>(
                getAllQuery(param),
                param,
                _transaction,
                commandType: CommandType.Text
            );            
        }

        /// <summary>
        /// Metodo base para consulta por id
        /// </summary>
        public async Task<TEntity?> GetByIdAsync(object param)
        {
            return
                await _connection.QueryFirstOrDefaultAsync<TEntity>(
                    getByIdQuery(param),
                    param,
                    _transaction,
                    commandType: CommandType.Text
                );
        }

        /// <summary>
        /// Metodo base para consulta de todos os registros
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAllFilterAsync(IEnumerable<Filter> filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            dynamic newFilter = new ExpandoObject();

            foreach (var f in filter)
            {
                ((IDictionary<string, object>)newFilter).Add(f.Name, f.Value);                
            }

            return
            await _connection.QueryAsync<TEntity>(
                getAllQueryFilter(filter, sortField, sortOrder, pageNumber, pageSize),
                (object)newFilter,
                _transaction,
                commandType: CommandType.Text
            );
        }       

        /// <summary>
        /// Metodo base para criação de novo registro
        /// </summary>
        public async Task<TMaitenanceResult> CreateAsync(object param)
        {
            var result = await _connection.QueryFirstAsync<TMaitenanceResult>(
                getCreateCommand(param),
                param,
                _transaction,
                commandType: CommandType.Text
            );            

            return result;            
        }

        /// <summary>
        /// Metodo base para alteração de um registro
        /// </summary>
        public async Task<TMaitenanceResult> ChangeAsync(object param)
        {            
            var result = await _connection.QueryFirstAsync<TMaitenanceResult>(
                getChangeCommand(param),
                param,
                _transaction,
                commandType: CommandType.Text
            );

            return result;         
        }

        /// <summary>
        /// Metodo base para exclusão de um registro
        /// </summary>
        public async Task<TMaitenanceResult> RemoveAsync(object key)
        {
            return await _connection.QueryFirstAsync<TMaitenanceResult>(
               getRemoveCommand(key),
               key,
               _transaction,
               commandType: CommandType.Text
            );
        }

        /// <summary>
        /// Metodo base para execução de comando
        /// </summary>
        public async Task<TMaitenanceResult> CommandAsync(object param, string command)
        {
            return
                await _connection.QueryFirstAsync<TMaitenanceResult>(
                    command,
                    param,
                    _transaction,
                    commandType: CommandType.Text
                );
        }

        /// <summary>
        /// Metodo base para execução de comando por stored procedure
        /// </summary>
        public async Task<TMaitenanceResult> CommandStProcAsync(object param, string stProc)
        {
            return
                await _connection.QueryFirstAsync<TMaitenanceResult>(
                    stProc,
                    param,
                    _transaction,
                    commandType: CommandType.StoredProcedure
                );
        }

        #region command/query building

        public virtual string getColumnFlagAtivo()
        {
            return "fl_atvo";
        }

        public virtual string getValorFlagAtivo()
        {
            return "S";
        }

        public virtual string getAllQuery(object param)
        {
            string sqlAllQuery = string.Format("select * from {0}", _table);

            if (_flagAtivo)
                sqlAllQuery = string.Concat(sqlAllQuery, " where {0} = '{1}'", getColumnFlagAtivo(), getValorFlagAtivo());

            if (param != null)
            {
                if (!sqlAllQuery.Contains("where "))
                    sqlAllQuery = string.Concat(sqlAllQuery, " where 1 = 1");

                var props = param.GetType().GetProperties();
                foreach ( var prop in props )
                {
                    sqlAllQuery = string.Concat(sqlAllQuery, string.Format(" and {0} = @{1}", prop.Name, prop.Name));
                }
            }

            return sqlAllQuery;
        }

        public virtual string getAllQueryFilterPrincipal(string sortField, string sortOrder)
        {
            return string.Format(@"select 
                                    nr_linh = row_number() over (order by {0}{1} {2}),
                                    * 
                                from 
                                    {3} t1 (nolock)",
                                    getColumnPrefix(sortField),
                                    sortField,
                                    sortOrder,
                                    _table);
        }

        public virtual string getAllQueryFilterCondition(IEnumerable<Filter> filter)
        {
            string sqlQueryCondition = "";

            if (filter != null)
            {
                if (!sqlQueryCondition.Contains("where "))
                    sqlQueryCondition = string.Concat(sqlQueryCondition, addQueryWhere(sqlQueryCondition));


                foreach (var f in filter)
                {
                    sqlQueryCondition = string.Concat(sqlQueryCondition, string.Format(getFilterQuery(f), string.Format("{0}{1}", getColumnPrefix(f.Name), f.Name), f.Name));
                }
            }

            return sqlQueryCondition;
        }

        public virtual string addQueryWhere(string query)
        {
            if (!query.Contains("where "))
                return " where 1 = 1";

            return "";
        }

        public virtual string getAllQueryFilter(IEnumerable<Filter> filter, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            string sqlAllQuery = "with tb_sele as (";

            sqlAllQuery = string.Concat(sqlAllQuery, getAllQueryFilterPrincipal(sortField, sortOrder));  

            if (_flagAtivo)
                sqlAllQuery = string.Concat(sqlAllQuery, " where {0} = '{1}'", getColumnFlagAtivo(), getValorFlagAtivo());

            sqlAllQuery = string.Concat(sqlAllQuery, getAllQueryFilterCondition(filter));

            sqlAllQuery = string.Concat(sqlAllQuery, ")");

            sqlAllQuery = string.Concat(sqlAllQuery, string.Format(@"select
                                                             *
                                                            ,rowscount	= (select max(nr_linh) from tb_sele)
                                                       from
                                                            tb_sele
                                                       where
                                                            nr_linh between 
                                                        (({0} * {1})  - ({2} -1)) and ({3} * {4})
                                                        ", pageNumber, pageSize, pageSize, pageNumber, pageSize));

            sqlAllQuery = string.Concat(sqlAllQuery, string.Format(" order by {0} {1}", sortField, sortOrder));
            
            return sqlAllQuery;
        }

        public virtual string getColumnPrefix(string? column)
        {
            return "";
        }

        public virtual string getFilterQuery(Filter filter)
        {
            switch(filter.MatchMode)
            {
                case "startsWith":
                    return " and {0} like @{1} + '%'";
                case "endsWith":
                    return " and {0} like '%' + @{1}";
                case "contains":
                    return " and {0} like '%' + @{1} + '%'";
                case "notContains":
                    return " and {0} not like '%' + @{1} + '%'";
                case "notEqual":
                    return " and {0} <> @{1}";
                default:
                    return " and {0} = @{1}";
            }
        }

        public virtual string getByIdQuery(object param)
        {
            string sqlByIdQuery = string.Format("select * from {0}", _table);
                        
            if (param != null)
            {
                if (!sqlByIdQuery.Contains("where "))
                    sqlByIdQuery = string.Concat(sqlByIdQuery, " where 1 = 1");

                var props = param.GetType().GetProperties();
                foreach (var prop in props)
                {
                    sqlByIdQuery = string.Concat(sqlByIdQuery, string.Format(" and {0} = @{1}", prop.Name, prop.Name));
                }
            }

            return sqlByIdQuery;
        }

        public virtual string getCreateCommand(object param)
        {
            var props = getPropsPersists(param);

            string sqlFields = String.Join(',', props.Select(p => p.Name).ToArray());
            string sqlValues = String.Join(',', props.Select(p => string.Concat("@", p.Name)).ToArray());

            string sqlCreateCommand = @"BEGIN TRY 
                                            INSERT {0} ({1}) values ({2});
                                            SELECT 0 AS return_code,'Registro criado com sucesso.' AS return_chav
                                        END TRY
                                        BEGIN CATCH
                                            SELECT @@ERROR AS return_code, ERROR_MESSAGE() AS return_chav
                                        END CATCH";                

            sqlCreateCommand = string.Format(sqlCreateCommand, _table, sqlFields, sqlValues);

            return sqlCreateCommand;
        }

        public virtual string getChangeCommand(object param)
        {
            var propsPersists = getPropsPersists(param, onlyUpdate: true);

            string sqlFieldsAndValues = "";

            foreach (var prop in propsPersists)
            {
                sqlFieldsAndValues = string.Concat(sqlFieldsAndValues,
                                                    sqlFieldsAndValues == "" ? "" : ",",
                                                    string.Format("{0} = @{1}", prop.Name, prop.Name));
            }
            var propsWhere = getPropsKey(param);

            string sqlWhere = "";

            foreach (var prop in propsWhere)
            {
                sqlWhere = string.Concat(sqlWhere,
                                            sqlWhere == "" ? "" : " AND ",
                                            string.Format("{0} = @{1}", prop.Name, prop.Name));
            }


            string sqlChangeCommand = @"BEGIN TRY 
                                            UPDATE {0} SET {1} WHERE {2};
                                            SELECT 0 AS return_code,'Registro alterado com sucesso.' AS return_chav
                                        END TRY
                                        BEGIN CATCH
                                            SELECT @@ERROR AS return_code, ERROR_MESSAGE() AS return_chav
                                        END CATCH";

            sqlChangeCommand = string.Format(sqlChangeCommand, _table, sqlFieldsAndValues, sqlWhere);

            return sqlChangeCommand;
        }

        public virtual string getRemoveCommand(object key)
        {
            var propsPersists = getPropsPersists(key, onlyUpdate: true);
                        
            var propsWhere = getPropsKey(key);

            string sqlWhere = "";

            foreach (var prop in propsWhere)
            {
                sqlWhere = string.Concat(sqlWhere,
                                            sqlWhere == "" ? "" : " AND ",
                                            string.Format("{0} = @{1}", prop.Name, prop.Name));
            }


            string sqlRemoveCommand = @"BEGIN TRY 
                                            DELETE {0} WHERE {1};
                                            SELECT 0 AS return_code,'Registro removido com sucesso.' AS return_chav
                                        END TRY
                                        BEGIN CATCH
                                            SELECT @@ERROR AS return_code, ERROR_MESSAGE() AS return_chav
                                        END CATCH";

            sqlRemoveCommand = string.Format(sqlRemoveCommand, _table, sqlWhere);

            return sqlRemoveCommand;
        }

        public virtual PropertyInfo[]? getPropsPersists(object param, bool onlyUpdate = false)
        {
            var props = param.GetType().GetProperties();

            List<PropertyInfo>? newProps = null;

            if (props.Length > 0)
            {
                newProps = new List<PropertyInfo>();                

                foreach (var prop in props)
                {
                    var entityAttrs = prop.GetCustomAttribute<EntityField>();
                    if (entityAttrs == null || (entityAttrs != null && entityAttrs.Persists))
                    {
                        if (onlyUpdate && entityAttrs != null && entityAttrs.IgnoreOnUpdate)
                            continue;

                        newProps.Add(prop);
                    }
                }
            }

            return newProps?.ToArray();
        }

        public virtual PropertyInfo[]? getPropsKey(object param)
        {
            var props = param.GetType().GetProperties();

            List<PropertyInfo>? newProps = null;

            if (props.Length > 0)
            {
                newProps = new List<PropertyInfo>();

                foreach (var prop in props)
                {
                    var entityAttrs = prop.GetCustomAttribute<EntityField>();
                    if (entityAttrs != null && entityAttrs.IsKey)
                        newProps.Add(prop);
                }
            }

            return newProps?.ToArray();
        }

        #endregion command/query building


    }
}