
namespace Global.Logging.Infrastructure.Repositories
{
    /// <inheritdoc cref="IAzBlobRepository{T}"/>
    public class AzBlobRepository<T> : AzStorageRepository, IAzBlobRepository<T>
         where T : class
    {
        /// <summary>
        /// Connection string of Azure storage 
        /// </summary>
        protected readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzBlobRepository{T}"/> class with the specified <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString">Connection string of Azure storage </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is null, empty, or white spaces.</exception>
        public AzBlobRepository(string connectionString)
        {
            AssertHelper.AssertNotNullNotWhiteSpaceOrThrow(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        #region Protected properties

        /// <summary>
        /// The Azure Blob Container name.
        /// </summary>
        protected string? BlobContainerName { get; private set; }

        /// <summary>
        /// The Azure Blob Container Client.
        /// </summary>
        protected BlobContainerClient? BlobContainerClient { get; private set; }

        /// <summary>
        /// The Azure Blob name.
        /// </summary>
        protected string? BlobName { get; private set; }

        /// <summary>
        /// The Azure Blob Client.
        /// </summary>
        protected BlobClient? BlobClient { get; private set; }

        #endregion

        #region Private methods

        private string BuildJsonSerializerName()
        {
            return typeof(JsonSerializer).FullName ?? nameof(JsonSerializer);
        }

        #endregion

        #region Asserts

        /// <summary>
        /// Asserts that the <see cref="BlobContainerClient"/> has been loaded.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobContainerClient"/> has not been loaded.</exception>
        protected virtual void AssertBlobContainerClientOrThrow()
        {
            AssertHelper.AssertNotNullOrThrow(BlobContainerClient, nameof(BlobContainerClient));
        }

        /// <summary>
        /// Asserts that the <see cref="BlobClient"/> has been loaded.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobClient"/> has not been loaded.</exception>
        protected virtual void AssertBlobClientOrThrow()
        {
            AssertHelper.AssertNotNullOrThrow(BlobClient, nameof(BlobClient));
        }

        /// <summary>
        /// Asserts necessary conditions before uploading content to the Azure Blob.
        /// </summary>
        /// <typeparam name="TCont">The type of content of the blob.</typeparam>
        /// <param name="content">The content to upload.</param>
        /// <param name="contentParamName">The parameter name of the content.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobContainerClient"/> has not been loaded.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobClient"/> has not been loaded.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="content"/> is null.</exception>
        protected virtual void AssertBeforeUploadOrThrow<TCont>(
            TCont content,
            string contentParamName)
            where TCont : class
        {
            AssertBlobContainerClientOrThrow();
            AssertBlobClientOrThrow();
            AssertHelper.AssertNotNullOrThrow(content, contentParamName);
        }

        /// <summary>
        /// Asserts necessary conditions before retrieving content from the blob.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobContainerClient"/> has not been loaded.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="BlobClient"/> has not been loaded.</exception>
        protected virtual void AssertBeforeGetOrThrow()
        {
            AssertBlobContainerClientOrThrow();
            AssertBlobClientOrThrow();
        }

        #endregion

        #region Build responses

        private AzBlobResponse BuildLoadBlobClientFailedResponse(RequestFailedException ex, string? blobContainerName, string? blobName)
        {
            var response = AzBlobResponseFactory.CreateFrom(ex);
            FillResponseMetadata(response, blobContainerName, blobName);

            return response;
        }

        private AzBlobResponse BuildLoadBlobClientFailedResponse(Exception ex, string? blobContainerName, string? blobName)
        {
            var response = AzBlobResponseFactory.CreateFailure(ex);
            FillResponseMetadata(response, blobContainerName, blobName);

            return response;
        }

        private AzBlobResponse BuildFailedAzBlobResponse(RequestFailedException ex)
        {
            var response = AzBlobResponseFactory.CreateFrom(ex);
            FillResponseMetadata(response);

            return response;
        }

        private AzBlobValueResponse<TOut> BuildFailedAzBlobValueResponse<TOut>(RequestFailedException ex)
        {
            var response = AzBlobValueResponseFactory.CreateFrom<TOut>(ex);
            FillResponseMetadata(response);

            return response;
        }

        private AzBlobValueResponse<TOut> BuildFailedAzBlobValueResponse<TOut>(Exception ex)
        {
            var response = AzBlobValueResponseFactory.CreateFailure<TOut>(ex);
            FillResponseMetadata(response);

            return response;
        }

        private AzBlobValueResponse<T> BuildNullDeserializerResultAzBlobValueResponse(string paramName)
        {
            var response = AzBlobValueResponseFactory.CreateFailure<T>();
            FillResponseMetadata(response);
            response.AddMessageTransactionally(RepositoryConstants.DeserializerErrorMessageKey,
                RepositoryConstants.NullDeserializerResult(paramName));

            return response;
        }

        private AzBlobValueResponse<string> BuildNullSerializerResultAzBlobValueResponse(string paramName)
        {
            var response = AzBlobValueResponseFactory.CreateFailure<string>();
            FillResponseMetadata(response);
            response.AddMessageTransactionally(RepositoryConstants.SerializerErrorMessageKey,
                RepositoryConstants.NullSerializerResult(paramName));

            return response;
        }

        #endregion

        #region Fill responses

        private void FillResponseMetadata<TAzureResponse, TEx>(IAzGlobalResponse<TAzureResponse, TEx> response)
            where TAzureResponse : IAzureResponse where TEx : Exception
        {
            FillResponseMetadata(response, BlobContainerName, BlobName);
        }

        private void FillResponseMetadata<TAzureResponse, TEx>(
            IAzGlobalResponse<TAzureResponse, TEx> response,
            string? blobContainerName,
            string? blobName)
            where TAzureResponse : IAzureResponse where TEx : Exception
        {
            response.AddMessageTransactionally(
                RepositoryConstants.BlobContainerNameMessageKey,
                blobContainerName ?? RepositoryConstants.BlobContainerNameNullOrEmptyMessage);
            response.AddMessageTransactionally(
                RepositoryConstants.BlobNameMessageKey,
                blobName ?? RepositoryConstants.BlobNameNullOrEmptyMessage);
        }

        #endregion

        #region Serialization methods

        // Sync

        /// <inheritdoc/>
        public virtual string DefaultJsonSerialize(
            T value,
            JsonSerializerOptions? options = null)
        {
            AssertHelper.AssertNotNullOrThrow(value, nameof(value));

            return JsonSerializer.Serialize(value, options);
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<string> Serialize(
            T value,
            JsonSerializerOptions? options = null)
        {
            try
            {
                var serializeResult = DefaultJsonSerialize(value, options);
                if (serializeResult == null)
                    return BuildNullSerializerResultAzBlobValueResponse(BuildJsonSerializerName());

                return AzBlobValueResponseFactory.CreateSuccessful(serializeResult!);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<string>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<string> Serialize(T value, Func<T, string> serialize)
        {
            AssertHelper.AssertNotNullOrThrow(value, nameof(value));
            AssertHelper.AssertNotNullOrThrow(serialize, nameof(serialize));

            try
            {
                var serializeResult = serialize(value);
                if (serializeResult == null)
                    return BuildNullSerializerResultAzBlobValueResponse(nameof(serialize));

                return AzBlobValueResponseFactory.CreateSuccessful(serializeResult!);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<string>(ex);
            }
        }

        // Async

        /// <inheritdoc/>
        public virtual async Task<string?> DefaultJsonSerializeAsync(
            T value,
            JsonSerializerOptions? options = null,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(value, nameof(value));

            using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, value, options, cancellationToken);

            string? result = default;
            memoryStream.TryConvertToString(ref result, encoding);

            return result;
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<string>> SerializeAsync(
            T value,
            JsonSerializerOptions? options = null,
            Encoding? encoding = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var serializeResult = await DefaultJsonSerializeAsync(value, options, encoding, cancellationToken);
                if (serializeResult == null)
                    return BuildNullSerializerResultAzBlobValueResponse(BuildJsonSerializerName());

                return AzBlobValueResponseFactory.CreateSuccessful(serializeResult!);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<string>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<string>> SerializeAsync(
            T value,
            Func<T, CancellationToken, Task<string>> serializeAsync,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(value, nameof(value));
            AssertHelper.AssertNotNullOrThrow(serializeAsync, nameof(serializeAsync));

            try
            {
                var serializeResult = await serializeAsync(value, cancellationToken);
                if (serializeResult == null)
                    return BuildNullSerializerResultAzBlobValueResponse(nameof(serializeAsync));

                return AzBlobValueResponseFactory.CreateSuccessful(serializeResult!);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<string>(ex);
            }
        }

        #endregion

        #region Deserialization methods

        // Sync

        /// <inheritdoc/>
        public virtual T? DefaultJsonDeserialize(
            Stream stream,
            JsonSerializerOptions? options = null)
        {
            AssertHelper.AssertNotNullOrThrow(stream, nameof(stream));

            return JsonSerializer.Deserialize<T>(stream, options);
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<T> Deserialize(
            Stream stream,
            JsonSerializerOptions? options = null)
        {
            try
            {
                var deserializeResult = DefaultJsonDeserialize(stream, options);
                if (deserializeResult == null)
                    return BuildNullDeserializerResultAzBlobValueResponse(BuildJsonSerializerName());

                var response = AzBlobValueResponseFactory.CreateSuccessful(deserializeResult!);
                FillResponseMetadata(response);

                return response;
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<T> Deserialize(Stream stream, Func<Stream, T?> deserialize)
        {
            AssertHelper.AssertNotNullOrThrow(stream, nameof(stream));
            AssertHelper.AssertNotNullOrThrow(deserialize, nameof(deserialize));

            try
            {
                var deserializeResult = deserialize(stream);
                if (deserializeResult == null)
                    return BuildNullDeserializerResultAzBlobValueResponse(nameof(deserialize));

                var response = AzBlobValueResponseFactory.CreateSuccessful(deserializeResult!);
                FillResponseMetadata(response);

                return response;
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        // Async

        /// <inheritdoc/>
        public virtual async Task<T?> DefaultJsonDeserializeAsync(
            Stream stream,
            JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(stream, nameof(stream));

            return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<T>> DeserializeAsync(
            Stream stream,
            JsonSerializerOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var deserializeResult = await DefaultJsonDeserializeAsync(stream, options, cancellationToken);
                if (deserializeResult == null)
                    return BuildNullDeserializerResultAzBlobValueResponse(BuildJsonSerializerName());

                var response = AzBlobValueResponseFactory.CreateSuccessful(deserializeResult!);
                FillResponseMetadata(response);

                return response;
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<T>> DeserializeAsync(
            Stream stream, 
            Func<Stream, CancellationToken, Task<T?>> deserializeAsync,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(stream, nameof(stream));
            AssertHelper.AssertNotNullOrThrow(deserializeAsync, nameof(deserializeAsync));

            try
            {
                var deserializeAsyncResult = await deserializeAsync(stream, cancellationToken);
                if (deserializeAsyncResult == null)
                    return BuildNullDeserializerResultAzBlobValueResponse(nameof(deserializeAsync));

                var response = AzBlobValueResponseFactory.CreateSuccessful(deserializeAsyncResult!);
                FillResponseMetadata(response);

                return response;
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        #endregion

        #region Load BlobContainerClient methods

        /// <inheritdoc/>
        public virtual AzBlobResponse LoadBlobContainerClient(
            string blobContainerName,
            bool createIfNotExists = true)
        {
            AzAssertHelper.AssertBlobContainerNameOrThrow(blobContainerName, nameof(blobContainerName));

            try
            {
                if (string.Equals(BlobContainerName, blobContainerName, StringComparison.InvariantCulture))
                    return AzBlobResponseFactory.CreateSuccessful();

                BlobContainerClient = new BlobContainerClient(_connectionString, blobContainerName, CreateClientOptionsOfGetDefault<AzBlobClientOptions>());

                AzBlobResponse response;
                if (createIfNotExists)
                {
                    // In Azure.Storage.Blobs (12.0.0), if a blob container already exists, CreateIfNotExists returns null without any documented reason.
                    var azureResponse = BlobContainerClient.CreateIfNotExists();
                    response = azureResponse != null ? AzBlobResponseFactory.CreateFrom(azureResponse) : AzBlobResponseFactory.CreateSuccessful();
                }
                else
                    response = AzBlobResponseFactory.CreateSuccessful();

                if (response.Status == ResponseStatus.Success)
                    BlobContainerName = blobContainerName;

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, blobContainerName, BlobName);
            }
            catch (Exception ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, blobContainerName, BlobName);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> LoadBlobContainerClientAsync(
            string blobContainerName,
            bool createIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken == default)
                    return await Task.Run(() => LoadBlobContainerClient(blobContainerName, createIfNotExists));

                return await Task.Run(() => LoadBlobContainerClient(blobContainerName, createIfNotExists), cancellationToken);
            }
            catch (Exception ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, blobContainerName, BlobName);
            }
        }

        #endregion

        #region Load BlobClient methods

        /// <inheritdoc/>
        public virtual AzBlobResponse LoadBlobClient(string blobName)
        {
            AssertBlobContainerClientOrThrow();
            AzAssertHelper.AssertDirectoryAndBlobNameOrThrow(blobName, nameof(blobName));

            try
            {
                if (string.IsNullOrEmpty(BlobName) || !BlobName.Equals(blobName))
                {
                    BlobClient = BlobContainerClient!.GetBlobClient(blobName);

                    BlobName = blobName;
                }

                return AzBlobResponseFactory.CreateSuccessful();
            }
            catch (RequestFailedException ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, BlobContainerName, blobName);
            }
            catch (Exception ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, BlobContainerName, blobName);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> LoadBlobClientAsync(
            string blobName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken ==  default)
                    return await Task.Run(() => LoadBlobClient(blobName));

                return await Task.Run(() => LoadBlobClient(blobName), cancellationToken);
            }
            catch (Exception ex)
            {
                return BuildLoadBlobClientFailedResponse(ex, BlobContainerName, blobName);
            }
        }

        #endregion

        #region Upload<T> methods

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Upload(
            T entity,
            JsonSerializerOptions? jsonSerializerOptions = null,
            bool overwrite = false)
        {
            AssertBeforeUploadOrThrow(entity, nameof(entity));

            try
            {
                var serializeResponse = Serialize(entity, jsonSerializerOptions);
                if (serializeResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<string, BlobContentInfo>(serializeResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return Upload(new BinaryData(serializeResponse.Value!), overwrite);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            T entity,
            JsonSerializerOptions? jsonSerializerOptions = null,
            Encoding? encoding = null,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeUploadOrThrow(entity, nameof(entity));

            try
            {
                var serializeResponse = await SerializeAsync(entity, jsonSerializerOptions, encoding, cancellationToken);
                if (serializeResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<string, BlobContentInfo>(serializeResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return await UploadAsync(new BinaryData(serializeResponse.Value!), overwrite, cancellationToken);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Upload(
            T entity,
            Func<T, string> serialize,
            bool overwrite = false)
        {
            AssertBeforeUploadOrThrow(entity, nameof(entity));
            AssertHelper.AssertNotNullOrThrow(serialize, nameof(serialize));

            try
            {
                var serializeResponse = Serialize(entity, serialize);
                if (serializeResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<string, BlobContentInfo>(serializeResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return Upload(new BinaryData(serializeResponse.Value!), overwrite);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            T entity,
            Func<T, CancellationToken, Task<string>> serializeAsync,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeUploadOrThrow(entity, nameof(entity));
            AssertHelper.AssertNotNullOrThrow(serializeAsync, nameof(serializeAsync));

            try
            {
                var serializeResponse = await SerializeAsync(entity, serializeAsync, cancellationToken);
                if (serializeResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<string, BlobContentInfo>(serializeResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return await UploadAsync(new BinaryData(serializeResponse.Value!), overwrite, cancellationToken);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        #endregion

        #region Upload methods

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Upload(BinaryData content, bool overwrite = false)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(BlobClient!.Upload(content, overwrite));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            BinaryData content,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(await BlobClient!.UploadAsync(content, overwrite, cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Upload(MemoryStream content, bool overwrite = false)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(BlobClient!.Upload(content, overwrite));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            MemoryStream content,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(await BlobClient!.UploadAsync(content, overwrite, cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobContentInfo> Upload(byte[] content, bool overwrite = false)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var convertToBinaryDataResponse = AzBlobRepositoryHelper.ConverTo(content);
                if (convertToBinaryDataResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<BlobContentInfo>(convertToBinaryDataResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return Upload(convertToBinaryDataResponse.Value!, overwrite);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            byte[] content,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeUploadOrThrow(content, nameof(content));

            try
            {
                var convertToBinaryDataResponse = AzBlobRepositoryHelper.ConverTo(content);
                if (convertToBinaryDataResponse.Status == ResponseStatus.Failure)
                {
                    var response = AzBlobValueResponseFactory.MapFromFailure<BlobContentInfo>(convertToBinaryDataResponse);
                    FillResponseMetadata(response);

                    return response;
                }

                return await UploadAsync(convertToBinaryDataResponse.Value!, overwrite, cancellationToken);
            }
            catch (Exception ex)
            {
                return BuildFailedAzBlobValueResponse<BlobContentInfo>(ex);
            }
        }

        #endregion

        #region Get methods

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<T> Get(
            JsonSerializerOptions? jsonSerializerOptions = null,
            BlobDownloadOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var getStreamResponse = DownloadStream(options);
                if (getStreamResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<Stream, T>(getStreamResponse);
                
                return Deserialize(getStreamResponse.Value!, jsonSerializerOptions);
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<T>> GetAsync(
            JsonSerializerOptions? jsonSerializerOptions = null,
            BlobDownloadOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var getStreamResponse = await DownloadStreamAsync(options, cancellationToken);
                if (getStreamResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<Stream, T>(getStreamResponse);
                
                return await DeserializeAsync(getStreamResponse.Value!, jsonSerializerOptions, cancellationToken);
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<T> Get(
            Func<Stream, T?> deserialize,
            BlobDownloadOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var getStreamResponse = DownloadStream(options);
                if (getStreamResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<Stream, T>(getStreamResponse);

                return Deserialize(getStreamResponse.Value!, deserialize);
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<T>> GetAsync(
            Func<Stream, CancellationToken, Task<T?>> deserializeAsync,
            BlobDownloadOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var getStreamResponse = await DownloadStreamAsync(options, cancellationToken);
                if (getStreamResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<Stream, T>(getStreamResponse);

                return await DeserializeAsync(getStreamResponse.Value!, deserializeAsync, cancellationToken);
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<T>(ex);
            }
        }

        #endregion

        #region Download methods

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobDownloadStreamingResult> DownloadStreaming(BlobDownloadOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(BlobClient!.DownloadStreaming(options));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadStreamingResult>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobDownloadStreamingResult>> DownloadStreamingAsync(
            BlobDownloadOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(await BlobClient!.DownloadStreamingAsync(options, cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadStreamingResult>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<Stream> DownloadStream(BlobDownloadOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var downloadStreamingResponse = DownloadStreaming(options);
                if (downloadStreamingResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<BlobDownloadStreamingResult, Stream>(downloadStreamingResponse);

                var response = AzBlobValueResponseFactory.CreateSuccessful(downloadStreamingResponse.Value!.Content);
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<Stream>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<Stream>> DownloadStreamAsync(
            BlobDownloadOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var downloadStreamingAsyncResponse = await DownloadStreamingAsync(options, cancellationToken);
                if (downloadStreamingAsyncResponse.Status == ResponseStatus.Failure)
                    return AzBlobValueResponseFactory.MapFromFailure<BlobDownloadStreamingResult, Stream>(downloadStreamingAsyncResponse);

                var response = AzBlobValueResponseFactory.CreateSuccessful(downloadStreamingAsyncResponse.Value!.Content);
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<Stream>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<BlobDownloadResult> DownloadContent(BlobDownloadOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(BlobClient!.DownloadContent(options));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadResult>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<BlobDownloadResult>> DownloadContentAsync(
            BlobDownloadOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(await BlobClient!.DownloadContentAsync(options, cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadResult>(ex);
            }
        }
        
        /// <inheritdoc/>
        public virtual AzBlobResponse DownloadTo(
            Stream destinationStream,
            BlobDownloadToOptions? options = null)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobResponseFactory.CreateFrom(BlobClient!.DownloadTo(destinationStream, options));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobResponse(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> DownloadToAsync(
            Stream destinationStream,
            BlobDownloadToOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobResponseFactory.CreateFrom(await BlobClient!.DownloadToAsync(destinationStream, options, cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobResponse(ex);
            }
        }

        /// <inheritdoc/>
        [Obsolete("This method includes 'BlobClient.Download()', which has been deprecated by Microsoft. Please consider using the options described in the summary.", false)]
        public virtual AzBlobValueResponse<BlobDownloadInfo> Download()
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(BlobClient!.Download());
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadInfo>(ex);
            }
        }

        /// <inheritdoc/>
        [Obsolete("This method includes 'BlobClient.DownloadAsync()', which has been deprecated by Microsoft. Please consider using the options described in the summary.", false)]
        public virtual async Task<AzBlobValueResponse<BlobDownloadInfo>> DownloadAsync(CancellationToken cancellationToken = default)
        {
            AssertBeforeGetOrThrow();

            try
            {
                var response = AzBlobValueResponseFactory.CreateFrom(await BlobClient!.DownloadAsync(cancellationToken));
                FillResponseMetadata(response);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzBlobValueResponse<BlobDownloadInfo>(ex);
            }
        }

        #endregion
    }
}
