
namespace Global.Logging.Tests
{
    internal class AzBlobServiceMock
    {
        private Mock<AzBlobService<LogEntity>> _mock;

        public AzBlobService<LogEntity> MockObject { get { return _mock.Object; } }

        public AzBlobServiceMock(IAzBlobRepository<LogEntity> azBlobRepository)
        {
            _mock = new Mock<AzBlobService<LogEntity>>(azBlobRepository) { CallBase = true };
        }

        #region Protected add methods

        List<LogEntity> _defaultAddLogEntities = new List<LogEntity>();
        public void SetDefaultAdd(
            LogEntity entity,
            AzBlobValueResponse<BlobContentInfo> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _defaultAddLogEntities.Add(entity);

            _mock.Protected().Setup<AzBlobValueResponse<BlobContentInfo>>("DefaultAdd", ItExpr.Is<LogEntity>(x => _defaultAddLogEntities.Where(ent => x.CompareTo(ent)).Count() > 0))
                .Returns(expectedResponse);
        }

        List<KeyValuePair<LogEntity, CancellationToken>> _defaultAddAsyncLogEntities = new List<KeyValuePair<LogEntity, CancellationToken>>();
        public void SetDefaultAddAsync(
            LogEntity entity,
            CancellationToken cancellationToken,
            AzBlobValueResponse<BlobContentInfo> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _defaultAddAsyncLogEntities.Add(new KeyValuePair<LogEntity, CancellationToken>(entity, cancellationToken));

            _mock.Protected().Setup<Task<AzBlobValueResponse<BlobContentInfo>>>("DefaultAddAsync",
                ItExpr.Is<LogEntity>(x => _defaultAddAsyncLogEntities.Where(ent => x.CompareTo(ent.Key)).Count() > 0),
                ItExpr.Is<CancellationToken>(x => _defaultAddAsyncLogEntities.Where(ent => x == ent.Value).Count() > 0))
                .Returns(Task.Run(() => expectedResponse));
        }

        int _defaultAddCallCount = 0;
        public void DefaultAdd(
            LogEntity entity,
            int callCountForExpectedResponse,
            AzBlobValueResponse<BlobContentInfo> expectedResponse,
            AzBlobValueResponse<BlobContentInfo> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<AzBlobValueResponse<BlobContentInfo>>("DefaultAdd", ItExpr.Is<LogEntity>(x => x.CompareTo(entity)))
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultAddCallCount)
                        return expectedResponse;

                    return defaultResponse;
                });
        }

        int _defaultAddAsyncCallCount = 0;
        public void DefaultAddAsync(
            LogEntity entity,
            CancellationToken cancellationToken,
            int callCountForExpectedResponse,
            AzBlobValueResponse<BlobContentInfo> expectedResponse,
            AzBlobValueResponse<BlobContentInfo> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<Task<AzBlobValueResponse<BlobContentInfo>>>("DefaultAddAsync", ItExpr.Is<LogEntity>(x => x.CompareTo(entity)), cancellationToken)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultAddAsyncCallCount)
                        return Task.Run(() => expectedResponse);

                    return Task.Run(() => defaultResponse);
                });
        }

        #endregion

        #region Protected get methods

        int _defaultGetCallCount = 0;
        public void DefaultGet(
            int callCountForExpectedResponse,
            AzBlobValueResponse<LogEntity> expectedResponse,
            AzBlobValueResponse<LogEntity> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<AzBlobValueResponse<LogEntity>>("DefaultGet", true)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultGetCallCount)
                        return expectedResponse;

                    return defaultResponse;
                });
        }

        int _defaultGetAsyncCallCount = 0;
        public void DefaultGetAsync(
            CancellationToken cancellationToken,
            int callCountForExpectedResponse,
            AzBlobValueResponse<LogEntity> expectedResponse,
            AzBlobValueResponse<LogEntity> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<Task<AzBlobValueResponse<LogEntity>>>("DefaultGetAsync", true, cancellationToken)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultGetAsyncCallCount)
                        return Task.Run(() => expectedResponse);

                    return Task.Run(() => defaultResponse);
                });
        }

        #endregion


        #region Protected get methods

        public void Set_GetWithRetry(
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            AzBlobValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Protected().Setup<AzBlobValueResponse<LogEntity>>("GetWithRetry", blobContainerName, blobName, maxRetryAttempts, createBlobContainerIfNotExists)
                .Returns(expectedResponse);
        }

        public void Set_GetWithRetryAsync(
            string blobContainerName,
            string blobName,
            int maxRetryAttempts,
            bool createBlobContainerIfNotExists,
            CancellationToken cancellationToken,
            AzBlobValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Protected().Setup<Task<AzBlobValueResponse<LogEntity>>>("GetWithRetryAsync", blobContainerName, blobName, maxRetryAttempts, createBlobContainerIfNotExists, cancellationToken)
                .Returns(Task.Run(() => expectedResponse));
        }

        #endregion
    }
}
