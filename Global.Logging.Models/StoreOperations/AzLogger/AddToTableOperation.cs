
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a store operation with response and raw log for adding to the Azure Table Service.
    /// </summary>
    /// <typeparam name="TRawLog">The type of raw log.</typeparam>
    /// <typeparam name="TEx">The type of exception associated with the <typeparamref name="TRawLog"/>.</typeparam>
    public class AddToTableOperation<TRawLog, TEx> : AddToStoreOperation<TRawLog, AzTableResponse, AzureTableServiceResponse, Exception>, ITableStoreOperation
        where TRawLog : IRawLog<TEx>
        where TEx : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddToTableOperation{TRawLog, TEx}"/> class.
        /// </summary>
        /// <param name="entity">The raw log.</param>
        /// <param name="sequenceExecutionType">The sequence execution type.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        public AddToTableOperation(
            TRawLog entity,
            SequenceExecutionType sequenceExecutionType,
            bool retryOnFailures = true,
            int? maxRetryAttempts = default,
            bool createTableIfNotExists = true) : base(entity, sequenceExecutionType, retryOnFailures, maxRetryAttempts)
        {
            CreateTableIfNotExists = createTableIfNotExists;
        }

        #region ITableStoreOperation

        /// <inheritdoc/>
        public virtual bool CreateTableIfNotExists { get; set; }

        #endregion
    }
}
