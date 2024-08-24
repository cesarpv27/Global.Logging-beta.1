
namespace Global.Logging.Models.StoreOperations.Abstractions
{
    /// <inheritdoc/>
    public abstract class StoreOperation<TResponse, TAzureResponse, TEx> : IStoreOperation<TResponse, TAzureResponse, TEx>
        where TResponse : IAzGlobalResponse<TAzureResponse, TEx>
        where TAzureResponse : IAzureResponse
        where TEx : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreOperation{TResponse, TAzureResponse, TEx}"/> class.
        /// </summary>
        /// <param name="sequenceExecutionType">The sequence execution type.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        public StoreOperation(
            SequenceExecutionType sequenceExecutionType,
            bool retryOnFailures,
            int? maxRetryAttempts)
        {
            SequenceExecutionType = sequenceExecutionType;
            RetryOnFailures = retryOnFailures;
            MaxRetryAttempts = maxRetryAttempts;
        }

        #region IStoreOperation

        /// <inheritdoc/>
        public virtual SequenceExecutionType SequenceExecutionType { get; protected set; }

        /// <inheritdoc/>
        public virtual bool RetryOnFailures { get; }

        /// <inheritdoc/>
        public virtual int? MaxRetryAttempts { get; }

        /// <inheritdoc/>
        public virtual bool HasResponse => Response != null;

        /// <inheritdoc/>
        public virtual TResponse? Response { get; protected set; }

        /// <inheritdoc/>
        public virtual void SetResponse(TResponse response)
        {
            AssertHelper.AssertNotNullOrThrow(response, nameof(response));

            Response = response;
        }

        #endregion

    }
}
