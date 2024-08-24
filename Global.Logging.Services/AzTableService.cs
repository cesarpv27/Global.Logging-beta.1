
namespace Global.Logging.Services
{
    /// <inheritdoc cref="IAzTableService{TLogEntity}"/>
    public class AzTableService<TLogEntity> : AzStorageService<TLogEntity>, IAzTableService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzTableService{TLogEntity}"/> class.
        /// </summary>
        /// <param name="azTableRepository">The Azure Table Service repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="azTableRepository"/> is null.</exception>
        public AzTableService(IAzTableRepository<TLogEntity> azTableRepository)
        {
            AssertHelper.AssertNotNullOrThrow(azTableRepository, nameof(azTableRepository));

            AzTableRepository = azTableRepository;
        }

        #region Public & protected properties

        /// <summary>
        /// The Azure Table Service repository.
        /// </summary>
        protected IAzTableRepository<TLogEntity> AzTableRepository { get; }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public virtual AzTableResponse Add(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true)
        {
            ServiceAssertHelper.AssertBeforeAddOrThrow(entity, tableName, retryOnFailures, maxRetryAttempts);

            try
            {
                return CustomAdd(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddAsync(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            ServiceAssertHelper.AssertBeforeAddOrThrow(entity, tableName, retryOnFailures, maxRetryAttempts);

            try
            {
                return await CustomAddAsync(entity, tableName, retryOnFailures, maxRetryAttempts, createTableIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzTableValueResponse<TLogEntity> Get(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false)
        {
            ServiceAssertHelper.AssertBeforeGetOrThrow(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts);

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
        public virtual async Task<AzTableValueResponse<TLogEntity>> GetAsync(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            ServiceAssertHelper.AssertBeforeGetOrThrow(tableName, partitionKey, rowKey, retryOnFailures, maxRetryAttempts);

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
        public virtual AzTableResponse SetAzRetryOptions(AzRetryOptions? azRetryOptions)
        {
            try
            {
                AzTableRepository.AzRetryOptions = azRetryOptions;

                return AzTableResponseFactory.CreateSuccessful();
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzTableValueResponse<AzRetryOptions?> GetAzRetryOptions()
        {
            try
            {
                if (AzTableRepository.AzRetryOptions != null)
                    return AzTableValueResponseFactory.CreateSuccessful<AzRetryOptions?>(AzTableRepository.AzRetryOptions);

                var response = AzTableValueResponseFactory.CreateWarning<AzRetryOptions?>(null);
                response.AddMessageTransactionally(nameof(AzRetryOptions), AzServiceConstants.AzRetryOptionsNotFoundMessage);

                return response;
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<AzRetryOptions?>(ex);
            }
        }

        #endregion

        #region Protected sync add methods

        /// <summary>
        /// Adds the specified entity to the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableResponse CustomAdd(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists) 
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzTableServiceConstants.DefaultAddAzTableMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return AddWithRetry(entity, tableName, maxRetryAttempts.Value, createTableIfNotExists);
        }

        /// <summary>
        /// Adds the specified entity to the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableResponse AddWithRetry(
            TLogEntity entity,
            string tableName,
            int maxRetryAttempts,
            bool createTableIfNotExists)
        {
            AzTableResponse response;
            int retryAttempts = 0;
            do
            {
                response = AzTableRepository.LoadTableClient(tableName, createTableIfNotExists);
                if(ShouldStopLoadOperation<AzTableResponse, AzureTableServiceResponse>(response))
                    break;
                if (response.Status != ResponseStatus.Failure)
                {
                    response = DefaultAdd(entity);
                    if (ShouldStopAddOperation<AzTableResponse, AzureTableServiceResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    Thread.Sleep(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            return response;
        }

        /// <summary>
        /// Adds the specified entity to the Azure Table Service.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableResponse DefaultAdd(TLogEntity entity)
        {
            try
            {
                return AzTableRepository.Add(entity);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        #endregion

        #region Protected async add methods

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzTableResponse> CustomAddAsync(
            TLogEntity entity,
            string tableName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists,
            CancellationToken cancellationToken = default)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzTableServiceConstants.DefaultAddAzTableMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return await AddWithRetryAsync(entity, tableName, maxRetryAttempts.Value, createTableIfNotExists, cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="tableName">The name of the table for storing the log entity.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzTableResponse> AddWithRetryAsync(
            TLogEntity entity,
            string tableName,
            int maxRetryAttempts,
            bool createTableIfNotExists,
            CancellationToken cancellationToken = default)
        {
            AzTableResponse response;
            int retryAttempts = 0;
            do
            {
                response = await AzTableRepository.LoadTableClientAsync(tableName, createTableIfNotExists, cancellationToken);
                if (ShouldStopLoadOperation<AzTableResponse, AzureTableServiceResponse>(response))
                    break;
                if (response.Status != ResponseStatus.Failure)
                {
                    response = await DefaultAddAsync(entity, cancellationToken);
                    if (ShouldStopAddOperation<AzTableResponse, AzureTableServiceResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    await Task.Delay(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            return response;
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Table Service.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzTableResponse> DefaultAddAsync(
            TLogEntity entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzTableRepository.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
        }

        #endregion

        #region Protected sync get methods

        /// <summary>
        /// Retrieves the entity from the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableValueResponse<TLogEntity> CustomGet(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createTableIfNotExists)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzTableServiceConstants.DefaultGetAzTableMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return GetWithRetry(tableName, partitionKey, rowKey, maxRetryAttempts.Value, createTableIfNotExists);
        }

        /// <summary>
        /// Retrieves the entity from the Azure Table Service with retries.
        /// For information about table names format, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableValueResponse<TLogEntity> GetWithRetry(
            string tableName,
            string partitionKey,
            string rowKey,
            int maxRetryAttempts,
            bool createTableIfNotExists)
        {
            AzTableValueResponse<TLogEntity>? response = null;
            AzTableResponse loadTableClientResponse;
            int retryAttempts = 0;
            do
            {
                loadTableClientResponse = AzTableRepository.LoadTableClient(tableName, createTableIfNotExists);
                if (ShouldStopLoadOperation<AzTableResponse, AzureTableServiceResponse>(loadTableClientResponse))
                    break;
                if (loadTableClientResponse.Status != ResponseStatus.Failure)
                {
                    response = DefaultGet(partitionKey, rowKey);
                    if (ShouldStopGetOperation<AzTableValueResponse<TLogEntity>, AzureTableServiceResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    Thread.Sleep(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzTableValueResponseFactory.MapFromFailure<TLogEntity>(loadTableClientResponse);
        }

        /// <summary>
        /// Retrieves the entity from the Azure Table Service.
        /// </summary>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzTableValueResponse<TLogEntity> DefaultGet(
            string partitionKey,
            string rowKey)
        {
            try
            {
                return AzTableRepository.Get(partitionKey, rowKey);
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        #endregion

        #region Protected async get methods

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Table Service with retries.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (default is true).</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
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
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzTableServiceConstants.DefaultGetAzTableMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return await GetWithRetryAsync(tableName, partitionKey, rowKey, maxRetryAttempts.Value, createTableIfNotExists, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Table Service with retries.
        /// </summary>
        /// <param name="tableName">The name of the table from where the log entity is retrieved.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createTableIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzTableValueResponse<TLogEntity>> GetWithRetryAsync(
            string tableName,
            string partitionKey,
            string rowKey,
            int maxRetryAttempts,
            bool createTableIfNotExists,
            CancellationToken cancellationToken = default)
        {
            AzTableValueResponse<TLogEntity>? response = null;
            AzTableResponse loadTableClientResponse;
            int retryAttempts = 0;
            do
            {
                loadTableClientResponse = await AzTableRepository.LoadTableClientAsync(tableName, createTableIfNotExists, cancellationToken);
                if (ShouldStopLoadOperation<AzTableResponse, AzureTableServiceResponse>(loadTableClientResponse))
                    break;
                if (loadTableClientResponse.Status != ResponseStatus.Failure)
                {
                    response = await DefaultGetAsync(partitionKey, rowKey, cancellationToken);
                    if (ShouldStopGetOperation<AzTableValueResponse<TLogEntity>, AzureTableServiceResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    await Task.Delay(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzTableValueResponseFactory.MapFromFailure<TLogEntity>(loadTableClientResponse);
        }

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Table Service.
        /// </summary>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzTableValueResponse<TLogEntity>> DefaultGetAsync(
            string partitionKey,
            string rowKey,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzTableRepository.GetAsync(partitionKey, rowKey, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        #endregion
    }
}
