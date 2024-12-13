namespace Admin.Domain.Interfaces.UoW
{
    /// <summary>
    /// A unidade de trabalho fornece transações compartilhadas para repositórios
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Identificador da unidade
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Necessário para criar e abrir uma nova conexão
        /// </summary>
        /// <returns>resultado assíncrono de <see cref="IUnitOfWork"/> aberta</returns>
        Task<IUnitOfWork> OpenConnectionAsync(string keyElement = "Database:ConnectionString");

        /// <summary>
        /// Cria uma conexão e a transação (se o método <see cref="OpenConnectionAsync"/> não foi executado)
        /// </summary>
        /// <remarks>
        ///     <para>Inicia uma transação para os repositórios</para>
        ///     <para>Use <see cref="SaveChangesAsync"/>> depois de executar</para>
        /// </remarks>
        /// <returns>resultado assíncrono de <see cref="IUnitOfWork"/> aberta</returns>
        Task<IUnitOfWork> BeginTransactionAsync(string keyElement = "Database:ConnectionString");

        /// <summary>
        /// Confirma se estiver ok ou reverte se lançar uma exceção
        /// </summary>
        /// <returns>async</returns>
        Task SaveChangesAsync();
    }
}