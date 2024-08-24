
namespace Global.Logging.Services
{
    /// <inheritdoc cref="IAzBlobService{TLogEntity}"/>
    public class AzBlobService<TLogEntity> : AzStorageService<TLogEntity>, IAzBlobService<TLogEntity>
        where TLogEntity : class, ILogEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzBlobService{TLogEntity}"/> class.
        /// </summary>
        /// <param name="azBlobRepository"></param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="azBlobRepository"/> is null.</exception>
        public AzBlobService(IAzBlobRepository<TLogEntity> azBlobRepository)
        {
            AssertHelper.AssertNotNullOrThrow(azBlobRepository, nameof(azBlobRepository));

            AzBlobRepository = azBlobRepository;
        }

        #region Public & protected properties

        /// <summary>
        /// The Azure Blob repository.
        /// </summary>
        protected IAzBlobRepository<TLogEntity> AzBlobRepository { get; private set; }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Add(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            ServiceAssertHelper.AssertBeforeAddOrThrow(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts);

            try
            {
                return CustomAdd(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> AddAsync(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            ServiceAssertHelper.AssertBeforeAddOrThrow(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts);

            try
            {
                return await CustomAddAsync(entity, blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<TLogEntity> Get(
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false)
        {
            ServiceAssertHelper.AssertBeforeGetOrThrow(blobContainerName, blobName, retryOnFailures, maxRetryAttempts);

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
        public virtual async Task<AzBlobValueResponse<TLogEntity>> GetAsync(
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            ServiceAssertHelper.AssertBeforeGetOrThrow(blobContainerName, blobName, retryOnFailures, maxRetryAttempts);

            try
            {
                return await CustomGetAsync(blobContainerName, blobName, retryOnFailures, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse SetAzRetryOptions(AzRetryOptions? azRetryOptions)
        {
            try
            {
                AzBlobRepository.AzRetryOptions = azRetryOptions;

                return AzBlobResponseFactory.CreateSuccessful();
            }
            catch (Exception ex)
            {
                return AzBlobResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<AzRetryOptions?> GetAzRetryOptions()
        {
            try
            {
                if (AzBlobRepository.AzRetryOptions != null)
                    return AzBlobValueResponseFactory.CreateSuccessful<AzRetryOptions?>(AzBlobRepository.AzRetryOptions);

                var response = AzBlobValueResponseFactory.CreateWarning<AzRetryOptions?>(null);
                response.AddMessageTransactionally(nameof(AzRetryOptions), AzServiceConstants.AzRetryOptionsNotFoundMessage);

                return response;
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<AzRetryOptions?>(ex);
            }
        }

        #endregion

        #region Protected sync add methods

        /// <summary>
        /// Adds the specified entity to the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual AzBlobValueResponse<BlobContentInfo> CustomAdd(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzBlobServiceConstants.DefaultAddAzBlobMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return AddWithRetry(entity, blobContainerName, blobName, maxRetryAttempts.Value, createBlobContainerIfNotExists);
        }

        /// <summary>
        /// Adds the specified entity to the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual AzBlobValueResponse<BlobContentInfo> AddWithRetry(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            AzBlobValueResponse<BlobContentInfo>? response = null;
            AzBlobResponse loadBlobResponse;
            int retryAttempts = 0;
            do
            {
                if (ShouldStopOperation_LoadingBlobContainerClient_LoadingBlobClient(blobContainerName, blobName, createBlobContainerIfNotExists, out loadBlobResponse))
                    break;
                if (loadBlobResponse.Status != ResponseStatus.Failure)
                {
                    response = DefaultAdd(entity);
                    if (ShouldStopAddOperation<AzBlobValueResponse<BlobContentInfo>, AzureBlobStorageResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    Thread.Sleep(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzBlobValueResponseFactory.MapFromFailure<BlobContentInfo>(loadBlobResponse);
        }

        /// <summary>
        /// Adds the specified entity to the Azure Blob Storage.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual AzBlobValueResponse<BlobContentInfo> DefaultAdd(TLogEntity entity)
        {
            try
            {
                return AzBlobRepository.Upload(entity);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>(ex);
            }
        }

        #endregion

        #region Protected async add methods

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual async Task<AzBlobValueResponse<BlobContentInfo>> CustomAddAsync(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzBlobServiceConstants.DefaultAddAzBlobMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return await AddWithRetryAsync(entity, blobContainerName, blobName, maxRetryAttempts.Value, createBlobContainerIfNotExists, cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual async Task<AzBlobValueResponse<BlobContentInfo>> AddWithRetryAsync(
            TLogEntity entity,
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            AzBlobValueResponse<BlobContentInfo>? response = null;
            (bool ShouldStopOperation, AzBlobResponse Response) shouldStopOperationResult;
            int retryAttempts = 0;
            do
            {
                shouldStopOperationResult = await ShouldStopOperationAsync_LoadingBlobContainerClientAsync_LoadingBlobClientAsync(blobContainerName, blobName, createBlobContainerIfNotExists, cancellationToken);
                if (shouldStopOperationResult.ShouldStopOperation)
                    break;
                if (shouldStopOperationResult.Response.Status != ResponseStatus.Failure)
                {
                    response = await DefaultAddAsync(entity, cancellationToken);
                    if (ShouldStopAddOperation<AzBlobValueResponse<BlobContentInfo>, AzureBlobStorageResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    await Task.Delay(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzBlobValueResponseFactory.MapFromFailure<BlobContentInfo>(shouldStopOperationResult.Response);
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the Azure Blob Storage.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        protected virtual async Task<AzBlobValueResponse<BlobContentInfo>> DefaultAddAsync(
            TLogEntity entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzBlobRepository.UploadAsync(entity, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>(ex);
            }
        }

        #endregion

        #region Protected sync get methods

        /// <summary>
        /// Retrieves the entity from the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzBlobValueResponse<TLogEntity> CustomGet(
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzBlobServiceConstants.DefaultGetAzBlobMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return GetWithRetry(blobContainerName, blobName, maxRetryAttempts.Value, createBlobContainerIfNotExists);
        }

        /// <summary>
        /// Retrieves the entity from the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzBlobValueResponse<TLogEntity> GetWithRetry(
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists)
        {
            AzBlobValueResponse<TLogEntity>? response = null;
            AzBlobResponse loadBlobResponse;
            int retryAttempts = 0;
            do
            {
                if (ShouldStopOperation_LoadingBlobContainerClient_LoadingBlobClient(blobContainerName, blobName, createBlobContainerIfNotExists, out loadBlobResponse))
                    break;
                if (loadBlobResponse.Status != ResponseStatus.Failure)
                {
                    response = DefaultGet();
                    if (ShouldStopGetOperation<AzBlobValueResponse<TLogEntity>, AzureBlobStorageResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    Thread.Sleep(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzBlobValueResponseFactory.MapFromFailure<TLogEntity>(loadBlobResponse);
        }

        /// <summary>
        /// Retrieves the entity from the Azure Blob Storage.
        /// </summary>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual AzBlobValueResponse<TLogEntity> DefaultGet()
        {
            try
            {
                return AzBlobRepository.Get();
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        #endregion

        #region Protected async get methods

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzBlobValueResponse<TLogEntity>> CustomGetAsync(
            string blobContainerName,
            string blobName,
            bool retryOnFailures,
            int? maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            maxRetryAttempts = AzServiceHelper.GetMaxRetryAttempts(
                AzBlobServiceConstants.DefaultGetAzBlobMaxRetryAttempts,
                retryOnFailures,
                maxRetryAttempts);

            return await GetWithRetryAsync(blobContainerName, blobName, maxRetryAttempts.Value, createBlobContainerIfNotExists, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Blob Storage with retries.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        protected virtual async Task<AzBlobValueResponse<TLogEntity>> GetWithRetryAsync(
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken = default)
        {
            AzBlobValueResponse<TLogEntity>? response = null;
            (bool ShouldStopOperation, AzBlobResponse Response) shouldStopOperationResult;
            int retryAttempts = 0;
            do
            {
                shouldStopOperationResult = await ShouldStopOperationAsync_LoadingBlobContainerClientAsync_LoadingBlobClientAsync(blobContainerName, blobName, createBlobContainerIfNotExists, cancellationToken);
                if (shouldStopOperationResult.ShouldStopOperation)
                    break;
                if (shouldStopOperationResult.Response.Status != ResponseStatus.Failure)
                {
                    response = await DefaultGetAsync(cancellationToken);
                    if (ShouldStopGetOperation<AzBlobValueResponse<TLogEntity>, AzureBlobStorageResponse>(response))
                        break;
                }

                if (++retryAttempts < maxRetryAttempts)
                    await Task.Delay(CalculateDelay(retryAttempts));
            }
            while (retryAttempts < maxRetryAttempts);

            if (response != null)
                return response;

            return AzBlobValueResponseFactory.MapFromFailure<TLogEntity>(shouldStopOperationResult.Response);
        }

        /// <summary>
        /// Asynchronously retrieves the entity from the Azure Blob Storage.
        /// </summary>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        protected virtual async Task<AzBlobValueResponse<TLogEntity>> DefaultGetAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzBlobRepository.GetAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TLogEntity>(ex);
            }
        }

        #endregion

        #region Private ShouldStopOperation loading Blob Container and loading Blob Client

        // Sync

        private bool ShouldStopOperation_LoadingBlobContainerClient_LoadingBlobClient(
            string blobContainerName,
            string blobName,
            bool createBlobContainerIfNotExists,
            out AzBlobResponse response)
        {
            response = AzBlobRepository.LoadBlobContainerClient(blobContainerName, createBlobContainerIfNotExists);
            if (ShouldStopLoadOperation<AzBlobResponse, AzureBlobStorageResponse>(response))
                return true;
            if (response.Status != ResponseStatus.Failure)
            {
                response = AzBlobRepository.LoadBlobClient(blobName);
                return ShouldStopLoadOperation<AzBlobResponse, AzureBlobStorageResponse>(response);
            }

            return false;
        }

        // Async

        private async Task<(bool ShouldStopOperation, AzBlobResponse Response)> ShouldStopOperationAsync_LoadingBlobContainerClientAsync_LoadingBlobClientAsync(
            string blobContainerName,
            string blobName,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken)
        {
            var response = await AzBlobRepository.LoadBlobContainerClientAsync(blobContainerName, createBlobContainerIfNotExists, cancellationToken);
            if (ShouldStopLoadOperation<AzBlobResponse, AzureBlobStorageResponse>(response))
                return (true, response);
            if (response.Status != ResponseStatus.Failure)
            {
                response = await AzBlobRepository.LoadBlobClientAsync(blobName, cancellationToken);
                return (ShouldStopLoadOperation<AzBlobResponse, AzureBlobStorageResponse>(response), response);
            }

            return (false, response);
        }

        #endregion
    }
}
