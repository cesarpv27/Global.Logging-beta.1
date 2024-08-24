
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a table service in Azure Storage.
    /// </summary>
    /// <typeparam name="TLogEntity">The type of log entity.</typeparam>
    /// <remarks>
    /// A table name in the correct format for Azure Table Service must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters or numbers.<br/>
    /// - The name must begin with a letter.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// - Some names are reserved, including 'tables'.<br/>
    /// </remarks>
    public interface IAzTableService<TLogEntity> : IAzStorageService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Adds the specified entity to the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the PartitionKey or the RowKey are null or empty in <paramref name="entity"/>, 
        /// or when <paramref name="tableName"/> has an incorrect format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        AzTableResponse Add(TLogEntity entity, string tableName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the PartitionKey or the RowKey are null or empty in <paramref name="entity"/>, 
        /// or when <paramref name="tableName"/> has an incorrect format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        Task<AzTableResponse> AddAsync(TLogEntity entity, string tableName, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the entity from the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service,
        /// or when the <paramref name="partitionKey"/> or the <paramref name="rowKey"/> are null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        AzTableValueResponse<TLogEntity> Get(string tableName, string partitionKey, string rowKey, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Table Service.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service,
        /// or when the <paramref name="partitionKey"/> or the <paramref name="rowKey"/> are null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        Task<AzTableValueResponse<TLogEntity>> GetAsync(string tableName, string partitionKey, string rowKey, bool retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the Azure retry options. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new table name for the TableClient.
        /// </summary>
        /// <param name="azRetryOptions">The retry options to set (Optional).</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the <paramref name="azRetryOptions"/> was successfully set; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableResponse SetAzRetryOptions(AzRetryOptions? azRetryOptions);

        /// <summary>
        /// Gets the Azure retry options. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// </summary>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the operation succeeded, the response contains the Status set to <see cref="ResponseStatus.Success"/> 
        /// and includes the <see cref="AzRetryOptions"/> or null if it does not contain an <see cref="AzRetryOptions"/>;
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableValueResponse<AzRetryOptions?> GetAzRetryOptions();
    }
}
