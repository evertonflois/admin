namespace Admin.Domain.Interfaces.UoW
{
    /// <summary>
    /// A unidade de trabalho fornece transa��es compartilhadas para reposit�rios
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Identificador da unidade
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Necess�rio para criar e abrir uma nova conex�o
        /// </summary>
        /// <returns>resultado ass�ncrono de <see cref="IUnitOfWork"/> aberta</returns>
        Task<IUnitOfWork> OpenConnectionAsync(string keyElement = "Database:ConnectionString");

        /// <summary>
        /// Cria uma conex�o e a transa��o (se o m�todo <see cref="OpenConnectionAsync"/> n�o foi executado)
        /// </summary>
        /// <remarks>
        ///     <para>Inicia uma transa��o para os reposit�rios</para>
        ///     <para>Use <see cref="SaveChangesAsync"/>> depois de executar</para>
        /// </remarks>
        /// <returns>resultado ass�ncrono de <see cref="IUnitOfWork"/> aberta</returns>
        Task<IUnitOfWork> BeginTransactionAsync(string keyElement = "Database:ConnectionString");

        /// <summary>
        /// Confirma se estiver ok ou reverte se lan�ar uma exce��o
        /// </summary>
        /// <returns>async</returns>
        Task SaveChangesAsync();
    }
}