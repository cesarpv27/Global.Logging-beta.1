
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a store operation with response and raw read-only for retrieving from the Azure Table Service.
    /// </summary>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    public class GetFromTableOperation<TReadOnlyLog> : StoreOperation<AzTableValueResponse<TReadOnlyLog>, AzureTableServiceResponse, Exception>, ITableStoreOperation
        where TReadOnlyLog : IReadOnlyLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddToTableOperation{TRawLog, TEx}"/> class.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="sequenceExecutionType">The sequence execution type.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        public GetFromTableOperation(
            string tableName,
            string partitionKey,
            string rowKey,
            SequenceExecutionType sequenceExecutionType,
            bool retryOnFailures = true,
            int? maxRetryAttempts = default,
            bool createTableIfNotExists = true) : base(sequenceExecutionType, retryOnFailures, maxRetryAttempts)
        {
            TableName = tableName;
            PartitionKey = partitionKey;
            RowKey = rowKey;
            CreateTableIfNotExists = createTableIfNotExists;
        }

        /// <summary>
        /// The Azure Table name.
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// The partition key of the entity.
        /// </summary>
        public virtual string PartitionKey { get; set; }

        /// <summary>
        /// The row key of the entity.
        /// </summary>
        public virtual string RowKey { get; set; }

        #region ITableStoreOperation

        /// <inheritdoc/>
        public virtual bool CreateTableIfNotExists { get; set; }

        #endregion
    }
}
