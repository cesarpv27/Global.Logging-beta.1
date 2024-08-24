
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a store operation.
    /// </summary>
    public interface IStoreOperation
    {
        /// <summary>
        /// Gets the sequence execution type.
        /// </summary>
        SequenceExecutionType SequenceExecutionType { get; }

        /// <summary>
        /// Flag indicating whether to retry on failures.
        /// </summary>
        bool RetryOnFailures { get; }

        /// <summary>
        /// The maximum number of retry attempts (optional).
        /// </summary>
        int? MaxRetryAttempts { get; }
    }

    /// <summary>
    /// Represents a store operation with response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the global response.</typeparam>
    /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
    /// <typeparam name="TEx">The type of the exception.</typeparam>
    public interface IStoreOperation<TResponse, TAzureResponse, TEx> : IStoreOperation
        where TResponse : IAzGlobalResponse<TAzureResponse, TEx>
        where TAzureResponse : IAzureResponse
        where TEx : Exception
    {
        /// <summary>
        /// Gets a value indicating whether the operation has a <see cref="Response"/>.
        /// </summary>
        bool HasResponse { get; }

        /// <summary>
        /// The response of the operation.
        /// </summary>
        TResponse? Response { get; }

        /// <summary>
        /// Sets the response for the store operation.
        /// </summary>
        /// <param name="response">The response to set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="response"/> is null.</exception>
        void SetResponse(TResponse response);
    }

    /// <summary>
    /// Represents a store operation with response and entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity involved in the store operation.</typeparam>
    /// <typeparam name="TResponse">The type of the global response.</typeparam>
    /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
    /// <typeparam name="TEx">The type of the exception.</typeparam>
    /// <inheritdoc/>
    public interface IAddToStoreOperation<TEntity, TResponse, TAzureResponse, TEx> : IStoreOperation<TResponse, TAzureResponse, TEx>
        where TResponse : IAzGlobalResponse<TAzureResponse, TEx>
        where TAzureResponse : IAzureResponse
        where TEx : Exception
    {
        /// <summary>
        /// Gets the log entity.
        /// </summary>
        TEntity LogEntity { get; }
    }
}
