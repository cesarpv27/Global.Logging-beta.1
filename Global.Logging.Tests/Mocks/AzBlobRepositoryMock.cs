
namespace Global.Logging.Tests
{
    public sealed class AzBlobRepositoryMock
    {
        private Mock<IAzBlobRepository<LogEntity>> _mock;

        public IAzBlobRepository<LogEntity> MockObject { get { return _mock.Object; } }

        public AzBlobRepositoryMock()
        {
            _mock = new Mock<IAzBlobRepository<LogEntity>>();
        }

        private AzBlobResponse GetResponseOrCreateSuccessful(AzBlobResponse? expectedResponse)
        {
            if (expectedResponse != default)
                return expectedResponse;
            return AzBlobResponseFactory.CreateSuccessful();
        }

        private AzBlobValueResponse<BlobContentInfo> GetResponseOrCreateSuccessful(AzBlobValueResponse<BlobContentInfo>? expectedResponse)
        {
            if (expectedResponse != default)
                return expectedResponse;
            return AzBlobValueResponseFactory.CreateSuccessful(RepositoryHelper.CreateDefaultBlobContentInfo());
        }

        #region Serialization methods

        public string DefaultJsonSerialize(LogEntity value, JsonSerializerOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<string> Serialize(LogEntity value, JsonSerializerOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<string> Serialize(LogEntity value, Func<LogEntity, string> serialize)
        {
            throw new NotImplementedException();
        }

        public Task<string?> DefaultJsonSerializeAsync(LogEntity value, JsonSerializerOptions? options = null, Encoding? encoding = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<string>> SerializeAsync(LogEntity value, JsonSerializerOptions? options = null, Encoding? encoding = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<string>> SerializeAsync(LogEntity value, Func<LogEntity, CancellationToken, Task<string>> serializeAsync, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Deserialization methods

        public LogEntity? DefaultJsonDeserialize(Stream stream, JsonSerializerOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<LogEntity> Deserialize(Stream stream, JsonSerializerOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<LogEntity> Deserialize(Stream stream, Func<Stream, LogEntity?> deserialize)
        {
            throw new NotImplementedException();
        }

        public Task<LogEntity?> DefaultJsonDeserializeAsync(Stream stream, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<LogEntity>> DeserializeAsync(Stream stream, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<LogEntity>> DeserializeAsync(Stream stream, Func<Stream, CancellationToken, Task<LogEntity?>> deserializeAsync, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Load BlobContainerClient methods

        public void SetLoadBlobContainerClient(
            string expectedBlobContainerName,
            bool expectedCreateIfNotExists,
            AzBlobResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadBlobContainerClient(expectedBlobContainerName, expectedCreateIfNotExists))
                .Returns(expectedResponse);
        }

        public void SetLoadBlobContainerClientAsync(
            string expectedBlobContainerName,
            bool expectedCreateIfNotExists,
            CancellationToken expectedCancellationToken,
            AzBlobResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadBlobContainerClientAsync(expectedBlobContainerName, expectedCreateIfNotExists, expectedCancellationToken))
                .Returns(Task.Run(() => expectedResponse));
        }

        public void LoadBlobContainerClient(
            string expectedBlobContainerName, 
            bool expectedCreateIfNotExists,
            AzBlobResponse? expectedResponse = null)
        {
            _mock.Setup(x => x.LoadBlobContainerClient(
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .Returns((
                    string inBlobContainerName,
                    bool inCreateIfNotExists) =>
                {
                    var response = AzBlobResponseFactory.CreateFailure();
                    if (!string.Equals(inBlobContainerName, expectedBlobContainerName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.BlobContainerNameMessageKey, expectedBlobContainerName);
                    else
                    if (inCreateIfNotExists != expectedCreateIfNotExists)
                        response.AddMessageTransactionally(MockConstants.CreateIfNotExistsMessageKey, expectedCreateIfNotExists.ToString());
                    else
                        return GetResponseOrCreateSuccessful(expectedResponse);

                    return response;
                });
        }

        public void LoadBlobContainerClientAsync(
            string expectedBlobContainerName, 
            bool expectedCreateIfNotExists, 
            CancellationToken expectedCancellationToken,
            AzBlobResponse? expectedResponse = null)
        {
            _mock.Setup(x => x.LoadBlobContainerClientAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns((
                    string inBlobContainerName,
                    bool inCreateIfNotExists,
                    CancellationToken inCancellationToken) =>
                {
                    var response = AzBlobResponseFactory.CreateFailure();
                    if (!string.Equals(inBlobContainerName, expectedBlobContainerName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.BlobContainerNameMessageKey, expectedBlobContainerName);
                    else
                    if (inCreateIfNotExists != expectedCreateIfNotExists)
                        response.AddMessageTransactionally(MockConstants.CreateIfNotExistsMessageKey, expectedCreateIfNotExists.ToString());
                    else
                    if (inCancellationToken != expectedCancellationToken)
                        response.AddMessageTransactionally(MockConstants.CancellationTokenMessageKey, MockConstants.CancellationTokenMessageKey);
                    else
                        return Task.Run(() => GetResponseOrCreateSuccessful(expectedResponse));

                    return Task.Run(() => response);
                });
        }

        #endregion

        #region Load BlobClient methods

        public void SetLoadBlobClient(
            string expectedBlobName,
            AzBlobResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadBlobClient(expectedBlobName))
                .Returns(expectedResponse);
        }

        public void SetLoadBlobClientAsync(
            string expectedBlobName,
            CancellationToken expectedCancellationToken,
            AzBlobResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadBlobClientAsync(expectedBlobName, expectedCancellationToken))
                .Returns(Task.Run(() => expectedResponse));
        }

        public void LoadBlobClient(
            string expectedBlobName,
            AzBlobResponse? expectedResponse = null)
        {
            _mock.Setup(x => x.LoadBlobClient(
                It.IsAny<string>()))
                .Returns((string inBlobName) =>
                {
                    var response = AzBlobResponseFactory.CreateFailure();
                    if (!string.Equals(inBlobName, expectedBlobName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.BlobNameMessageKey, expectedBlobName);
                    else
                        return GetResponseOrCreateSuccessful(expectedResponse);

                    return response;
                });
        }

        public void LoadBlobClientAsync(
            string expectedBlobName, 
            CancellationToken expectedCancellationToken,
            AzBlobResponse? expectedResponse = null)
        {
            _mock.Setup(x => x.LoadBlobClientAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .Returns((
                    string inBlobName,
                    CancellationToken inCancellationToken) =>
                {
                    var response = AzBlobResponseFactory.CreateFailure();
                    if (!string.Equals(inBlobName, expectedBlobName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.BlobNameMessageKey, expectedBlobName);
                    else
                    if (inCancellationToken != expectedCancellationToken)
                        response.AddMessageTransactionally(MockConstants.CancellationTokenMessageKey, MockConstants.CancellationTokenMessageKey);
                    else
                        return Task.Run(() => GetResponseOrCreateSuccessful(expectedResponse));

                    return Task.Run(() => response);
                });
        }

        #endregion

        #region Upload<LogEntity> methods

        public void Upload(
            LogEntity expectedEntity, 
            JsonSerializerOptions? expectedJsonSerializerOptions, 
            bool expectedOverwrite,
            AzBlobValueResponse<BlobContentInfo>? expectedResponse = null)
        {
            _mock.Setup(x => x.Upload(
                It.IsAny<LogEntity>(),
                It.IsAny<JsonSerializerOptions?>(),
                It.IsAny<bool>()))
                .Returns((
                    LogEntity inEntity,
                    JsonSerializerOptions? inJsonSerializerOptions,
                    bool inOverwrite) =>
                {
                    var response = AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>();
                    if (!inEntity.CompareTo(expectedEntity))
                    {
                        response.AddMessageTransactionally(MockConstants.EntityPartitionKeyMessageKey, expectedEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.EntityRowKeyMessageKey, expectedEntity.RowKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityPartitionKeyMessageKey, inEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityRowKeyMessageKey, inEntity.RowKey);
                    }
                    else
                    if (inJsonSerializerOptions != expectedJsonSerializerOptions)
                        response.AddMessageTransactionally(MockConstants.JsonSerializerOptionsMessageKey, nameof(expectedJsonSerializerOptions));
                    else
                    if (inOverwrite != expectedOverwrite)
                        response.AddMessageTransactionally(MockConstants.OverwriteMessageKey, expectedOverwrite.ToString());
                    else
                        return GetResponseOrCreateSuccessful(expectedResponse);

                    return response;
                });
        }

        public void UploadAsync(
            LogEntity expectedEntity, 
            JsonSerializerOptions? expectedJsonSerializerOptions, 
            Encoding? expectedEncoding, 
            bool expectedOverwrite, 
            CancellationToken expectedCancellationToken,
            AzBlobValueResponse<BlobContentInfo>? expectedResponse = null)
        {
            _mock.Setup(x => x.UploadAsync(
                It.IsAny<LogEntity>(),
                It.IsAny<JsonSerializerOptions?>(),
                It.IsAny<Encoding>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns((
                    LogEntity inEntity,
                    JsonSerializerOptions? inJsonSerializerOptions,
                    Encoding? inEncoding,
                    bool inOverwrite,
                    CancellationToken inCancellationToken) =>
                {
                    var response = AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>();
                    if (!inEntity.CompareTo(expectedEntity))
                    {
                        response.AddMessageTransactionally(MockConstants.EntityPartitionKeyMessageKey, expectedEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.EntityRowKeyMessageKey, expectedEntity.RowKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityPartitionKeyMessageKey, inEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityRowKeyMessageKey, inEntity.RowKey);
                    }
                    else
                    if (inJsonSerializerOptions != expectedJsonSerializerOptions)
                        response.AddMessageTransactionally(MockConstants.JsonSerializerOptionsMessageKey, nameof(expectedJsonSerializerOptions));
                    else
                    if (inOverwrite != expectedOverwrite)
                        response.AddMessageTransactionally(MockConstants.OverwriteMessageKey, expectedOverwrite.ToString());
                    else
                    if (inEncoding != expectedEncoding)
                        response.AddMessageTransactionally(MockConstants.EncodingMessageKey, nameof(expectedEncoding));
                    else
                    if (inCancellationToken != expectedCancellationToken)
                        response.AddMessageTransactionally(MockConstants.CancellationTokenMessageKey, expectedCancellationToken.ToString() ?? MockConstants.CancellationTokenMessageKey);
                    else
                        return Task.Run(() => GetResponseOrCreateSuccessful(expectedResponse));

                    return Task.Run(() => response);
                });
        }

        public AzBlobValueResponse<BlobContentInfo> Upload(
            LogEntity entity, 
            Func<LogEntity, string> serialize, 
            bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(
            LogEntity entity, 
            Func<LogEntity, CancellationToken, Task<string>> serializeAsync, 
            bool overwrite = false, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Upload methods

        public AzBlobValueResponse<BlobContentInfo> Upload(BinaryData content, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(BinaryData content, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<BlobContentInfo> Upload(MemoryStream content, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(MemoryStream content, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<BlobContentInfo> Upload(byte[] content, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(byte[] content, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Get methods

        public void Get(
            JsonSerializerOptions? serializerOptions, 
            BlobDownloadOptions? options,
            AzBlobValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.Get(
                It.Is<JsonSerializerOptions?>(sOpt => sOpt == serializerOptions),
                It.Is<BlobDownloadOptions?>(opt => opt == options)))
                .Returns(expectedResponse);
        }

        public void GetAsync(
            JsonSerializerOptions? serializerOptions, 
            BlobDownloadOptions? options, 
            CancellationToken cancellationToken,
            AzBlobValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.GetAsync(
                It.Is<JsonSerializerOptions?>(sOpt => sOpt == serializerOptions),
                It.Is<BlobDownloadOptions?>(opt => opt == options),
                It.Is<CancellationToken>(x => x == cancellationToken)))
                .Returns(Task.Run(() => expectedResponse));
        }

        public AzBlobValueResponse<LogEntity> Get(Func<Stream, LogEntity?> deserialize, BlobDownloadOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<LogEntity>> GetAsync(Func<Stream, CancellationToken, Task<LogEntity?>> deserializeAsync, BlobDownloadOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Download methods

        public AzBlobValueResponse<BlobDownloadStreamingResult> DownloadStreaming(BlobDownloadOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobDownloadStreamingResult>> DownloadStreamingAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<Stream> DownloadStream(BlobDownloadOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<Stream>> DownloadStreamAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<BlobDownloadResult> DownloadContent(BlobDownloadOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobDownloadResult>> DownloadContentAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobResponse DownloadTo(Stream destinationStream, BlobDownloadToOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobResponse> DownloadToAsync(Stream destinationStream, BlobDownloadToOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public AzBlobValueResponse<BlobDownloadInfo> Download()
        {
            throw new NotImplementedException();
        }

        public Task<AzBlobValueResponse<BlobDownloadInfo>> DownloadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
