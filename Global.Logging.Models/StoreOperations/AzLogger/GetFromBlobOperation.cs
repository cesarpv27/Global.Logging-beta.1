
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a store operation with response and read-only log for retrieving from the Azure Blob Storage.
    /// </summary>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    public class GetFromBlobOperation<TReadOnlyLog> : StoreOperation<AzBlobValueResponse<TReadOnlyLog>, AzureBlobStorageResponse, Exception>, IBlobStoreOperation
        where TReadOnlyLog : IReadOnlyLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFromBlobOperation{TReadOnlyLog}"/> class.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="sequenceExecutionType">The sequence execution type.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        public GetFromBlobOperation(
            string blobContainerName,
            string blobName,
            SequenceExecutionType sequenceExecutionType,
            bool retryOnFailures = true,
            int? maxRetryAttempts = default,
            bool createBlobContainerIfNotExists = true) : base(sequenceExecutionType, retryOnFailures, maxRetryAttempts)
        {
            BlobContainerName = blobContainerName;
            BlobName = blobName;

            CreateBlobContainerIfNotExists = createBlobContainerIfNotExists;
        }

        /// <summary>
        /// The Azure Blob Container name.
        /// </summary>
        public virtual string BlobContainerName { get; set; }

        /// <summary>
        /// The Azure Blob name.
        /// </summary>
        public virtual string BlobName { get; set; }

        #region IBlobStoreOperation

        /// <inheritdoc/>
        public virtual bool CreateBlobContainerIfNotExists { get; set; }

        #endregion
    }
}
