
namespace Global.Logging.Models.StoreOperations.Abstractions
{
    /// <inheritdoc/>
    public abstract class AddToStoreOperation<TEntity, TResponse, TAzureResponse, TEx> : StoreOperation<TResponse, TAzureResponse, TEx>, IAddToStoreOperation<TEntity, TResponse, TAzureResponse, TEx>
        where TResponse : IAzGlobalResponse<TAzureResponse, TEx>
        where TAzureResponse : IAzureResponse
        where TEx : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddToStoreOperation{TEntity, TResponse, TAzureResponse, TEx}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="sequenceExecutionType">The sequence execution type.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        public AddToStoreOperation(
            TEntity entity,
            SequenceExecutionType sequenceExecutionType,
            bool retryOnFailures,
            int? maxRetryAttempts) : base(sequenceExecutionType, retryOnFailures, maxRetryAttempts)
        {
            AssertHelper.AssertNotNullOrThrow(entity, nameof(entity));

            LogEntity = entity;
        }

        /// <inheritdoc/>
        public virtual TEntity LogEntity { get; }
    }
}
