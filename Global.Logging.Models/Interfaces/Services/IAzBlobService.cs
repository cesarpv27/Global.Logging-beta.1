
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a blob service in Azure Storage.
    /// </summary>
    /// <typeparam name="TLogEntity">The type of log entity.</typeparam>
    /// <remarks>
    /// A blob container name in the correct format for Azure Blob Storage must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters, numbers, or the '-' character.<br/>
    /// - All characters must be lowercase.<br/>
    /// - The first and last characters must be a letter or a number.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// </remarks>
    public interface IAzBlobService<TLogEntity> : IAzStorageService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Adds the specified entity to the Azure Blob Storage.
        /// For information about blob container names and blob names format, visit https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the 'PartitionKey' or the 'RowKey' are null or empty in <paramref name="entity"/>, 
        /// or when <paramref name="blobContainerName"/> or <paramref name="blobName"/> have an incorrect format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        AzBlobValueResponse<BlobContentInfo> Add(TLogEntity entity, string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Blob Storage.
        /// For information about blob container names and blob names format, visit https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the 'PartitionKey' or the 'RowKey' are null or empty in <paramref name="entity"/>, 
        /// or when <paramref name="blobContainerName"/> or <paramref name="blobName"/> have an incorrect format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> AddAsync(TLogEntity entity, string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the entity from the Azure Blob Storage.
        /// For information about blob container names and blob names format, visit https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobContainerName"/> 
        /// or <paramref name="blobName"/> have an incorrect format for Azure Blob Storage.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        AzBlobValueResponse<TLogEntity> Get(string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Blob Storage.
        /// For information about blob container names and blob names format, visit https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobContainerName"/> 
        /// or <paramref name="blobName"/> have an incorrect format for Azure Blob Storage.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        Task<AzBlobValueResponse<TLogEntity>> GetAsync(string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the Azure retry options. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new blob container name for the BlobContainerClient.
        /// </summary>
        /// <param name="azRetryOptions">The retry options to set (Optional).</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the <paramref name="azRetryOptions"/> was successfully set; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobResponse SetAzRetryOptions(AzRetryOptions? azRetryOptions);

        /// <summary>
        /// Gets the Azure retry options. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// </summary>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If the operation succeeded, the response contains the Status set to <see cref="ResponseStatus.Success"/> 
        /// and includes the <see cref="AzRetryOptions"/> or null if it does not contain an <see cref="AzRetryOptions"/>;
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobValueResponse<AzRetryOptions?> GetAzRetryOptions();
    }
}
