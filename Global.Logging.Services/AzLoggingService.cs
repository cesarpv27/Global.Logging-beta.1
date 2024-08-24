
namespace Global.Logging.Services
{
    /// <inheritdoc cref="IAzLoggingService{TLogEntity}"/>
    public class AzLoggingService<TLogEntity> : IAzLoggingService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzLoggingService{TLogEntity}"/> class.
        /// </summary>
        /// <param name="azTableService">The Azure Table service used for logging.</param>
        /// <param name="azBlobService">The Azure Blob service used for logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="azTableService"/> or <paramref name="azBlobService"/> are null.</exception>
        public AzLoggingService(
            IAzTableService<TLogEntity> azTableService,
            IAzBlobService<TLogEntity> azBlobService)
        {
            AssertHelper.AssertNotNullOrThrow(azTableService, nameof(azTableService));
            AssertHelper.AssertNotNullOrThrow(azBlobService, nameof(azBlobService));

            AzTableService = azTableService;
            AzBlobService = azBlobService;
        }

        #region Protected properties

        /// <summary>
        /// The Table Service instance.
        /// </summary>
        protected virtual IAzTableService<TLogEntity> AzTableService { get; private set; }

        /// <summary>
        /// The Blob Service instance.
        /// </summary>
        protected virtual IAzBlobService<TLogEntity> AzBlobService { get; private set; }

        #endregion

        #region Update params methods

        /// <inheritdoc/>
        public virtual AzTableResponse SetAzTableRetryOptions(AzRetryOptions? azRetryOptions)
        {
            return AzTableService.SetAzRetryOptions(azRetryOptions);
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse SetAzBlobRetryOptions(AzRetryOptions? azRetryOptions)
        {
            return AzBlobService.SetAzRetryOptions(azRetryOptions);
        }

        #endregion

        #region Public add methods

        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true)
        {
            try
            {
                return CustomAddToTable(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await CustomAddToTableAsync(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            try
            {
                return CustomAddToBlob(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists);
            }
            catch (Exception ex)
            {
                return AzBlobResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await CustomAddToBlobAsync(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobResponseFactory.CreateFailure(ex);
            }
        }

        #endregion

        #region Public get methods

        /// <inheritdoc/>
        public virtual AzTableValueResponse<TLogEntity> GetFromTable(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false)
        {
            try
            {
                return CustomGet(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts, createTableIfNotExists);
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableValueResponse<TLogEntity>> GetFromTableAsync(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await CustomGetAsync(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts, createTableIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<TLogEntity> GetFromBlob(
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false)
        {
            try
            {
                return CustomGet(blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<TLogEntity>> GetFromBlobAsync(
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await CustomGetAsync(blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        #endregion

        #region Protected adds methods

        /// <summary>
        /// Adds the specified log entity to an Azure Table Service.
        /// </summary>
        /// <param name="entity">The log entity to add to the Azure Table Service.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableResponse CustomAddToTable(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists)
        {
            return AzTableService.Add(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists);
        }

        /// <summary>
        /// Asynchronously adds the specified log entity to an Azure Table Service.
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
        protected virtual async Task<AzTableResponse> CustomAddToTableAsync(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists,
            CancellationToken cancellationToken = default)
        {
            return await AzTableService.AddAsync(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists, cancellationToken);
        }

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
        protected virtual AzBlobResponse CustomAddToBlob(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            AzBlobValueResponse<BlobContentInfo> addResponse = AzBlobService.Add(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists);

            return MapFrom(addResponse);
        }

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
        protected virtual async Task<AzBlobResponse> CustomAddToBlobAsync(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            AzBlobValueResponse<BlobContentInfo> addResponse = await AzBlobService.AddAsync(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);

            return MapFrom(addResponse);
        }

        #endregion

        #region Protected get methods

        /// <summary>
        /// Retrieves the log entity from the Azure Table Service.
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
        protected virtual AzTableValueResponse<TLogEntity> CustomGet(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists)
        {
            return AzTableService.Get(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts, createTableIfNotExists);
        }

        /// <summary>
        /// Asynchronously retrieve the log entity from the Azure Table Service.
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
        protected virtual async Task<AzTableValueResponse<TLogEntity>> CustomGetAsync(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists,
            CancellationToken cancellationToken = default)
        {
            return await AzTableService.GetAsync(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts, createTableIfNotExists, cancellationToken);
        }

        /// <summary>
        /// Retrieves the log entity from the Azure Blob Storage.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzBlobValueResponse<TLogEntity> CustomGet(
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            return AzBlobService.Get(blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists);
        }

        /// <summary>
        /// Asynchronously retrieves the log entity from the Azure Blob Storage.
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
        protected virtual async Task<AzBlobValueResponse<TLogEntity>> CustomGetAsync(
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            return await AzBlobService.GetAsync(blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);
        }

        #endregion

        #region Private methods

        private AzBlobResponse MapFrom(AzBlobValueResponse<BlobContentInfo> azBlobValueResponse)
        {
            if (azBlobValueResponse.Status == ResponseStatus.Failure)
                return AzBlobResponseFactory.MapFromFailure(azBlobValueResponse);

            return AzBlobResponseFactory.CreateFrom(azBlobValueResponse);
        }

        #endregion
    }
}
