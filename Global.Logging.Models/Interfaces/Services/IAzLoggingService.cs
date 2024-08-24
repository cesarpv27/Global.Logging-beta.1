
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a logging service in Azure Storage.
    /// </summary>
    /// <remarks>
    /// A table name in the correct format for Azure Table Service must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters or numbers.<br/>
    /// - The name must begin with a letter.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// - Some names are reserved, including 'tables'.<br/>
    /// <br/>
    /// A blob container name in the correct format for Azure Blob Storage must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters, numbers, or the '-' character.<br/>
    /// - All characters must be lowercase.<br/>
    /// - The first and last characters must be a letter or a number.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// <br/>
    /// The <see cref="IAzLoggingService{TLogEntity}"/> can be managed and configured using the following methods and properties:<br/>
    /// <see cref="SetAzTableRetryOptions"/><br/>
    /// <see cref="SetAzBlobRetryOptions"/><br/>
    /// </remarks>
    /// <typeparam name="TLogEntity">The type of log entity.</typeparam>
    public interface IAzLoggingService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Adds the specified log entity to an Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The log entity to add to the Azure Table Service.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableResponse AddToTable(TLogEntity entity, string tableName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds the specified log entity to an Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The log entity to add to the Azure Table Service.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzTableResponse> AddToTableAsync(TLogEntity entity, string tableName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified log entity to an Azure Blob Storage.
        /// </summary>
        /// <param name="entity">The log entity to add to the Azure Blob Storage.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobResponse AddToBlob(TLogEntity entity, string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds the specified log entity to an Azure Blob Storage.
        /// </summary>
        /// <param name="entity">The log entity to add to the Azure Blob Storage.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzBlobResponse> AddToBlobAsync(TLogEntity entity, string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the log entity from the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableValueResponse<TLogEntity> GetFromTable(string tableName, string partitionKey, string rowKey, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieve the log entity from the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzTableValueResponse<TLogEntity>> GetFromTableAsync(string tableName, string partitionKey, string rowKey, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the log entity from the Azure Blob Storage.
        /// For information about blob container names and blob names format, visit https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobValueResponse<TLogEntity> GetFromBlob(string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieves the log entity from the Azure Blob Storage.
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
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzBlobValueResponse<TLogEntity>> GetFromBlobAsync(string blobContainerName, string blobName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the <paramref name="azRetryOptions"/> to the <see cref="IAzTableService{TLogEntity}"/>. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new table name for the TableClient.
        /// </summary>
        /// <param name="azRetryOptions">The retry options instance for Azure Table Service.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the <paramref name="azRetryOptions"/> was successfully set; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableResponse SetAzTableRetryOptions(AzRetryOptions? azRetryOptions);

        /// <summary>
        /// Sets the <paramref name="azRetryOptions"/> to the <see cref="IAzBlobService{TLogEntity}"/>. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new blob container name for the BlobContainerClient.
        /// </summary>
        /// <param name="azRetryOptions">The retry options instance.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the <paramref name="azRetryOptions"/> was successfully set; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobResponse SetAzBlobRetryOptions(AzRetryOptions? azRetryOptions);
    }
}
