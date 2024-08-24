
namespace Global.Logging.Tests
{
    internal class AzTableServiceMock
    {
        private Mock<AzTableService<LogEntity>> _mock;

        public AzTableService<LogEntity> MockObject { get { return _mock.Object; } }

        public AzTableServiceMock(IAzTableRepository<LogEntity> azTableRepository)
        {
            _mock = new Mock<AzTableService<LogEntity>>(azTableRepository) { CallBase = true};
        }

        #region Protected add methods

        List<LogEntity> _defaultAddLogEntities = new List<LogEntity>();
        public void SetDefaultAdd(
            LogEntity entity,
            AzTableResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _defaultAddLogEntities.Add(entity);

            _mock.Protected().Setup<AzTableResponse>("DefaultAdd", ItExpr.Is<LogEntity>(x => _defaultAddLogEntities.Where(ent => x.CompareTo(ent)).Count() > 0))
                .Returns(expectedResponse);
        }

        List<KeyValuePair<LogEntity, CancellationToken>> _defaultAddAsyncLogEntities = new List<KeyValuePair<LogEntity, CancellationToken>>();
        public void SetDefaultAddAsync(
            LogEntity entity,
            CancellationToken cancellationToken,
            AzTableResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _defaultAddAsyncLogEntities.Add(new KeyValuePair<LogEntity, CancellationToken>(entity, cancellationToken));

            _mock.Protected().Setup<Task<AzTableResponse>>("DefaultAddAsync", 
                ItExpr.Is<LogEntity>(x => _defaultAddAsyncLogEntities.Where(ent => x.CompareTo(ent.Key)).Count() > 0),
                ItExpr.Is<CancellationToken>(x => _defaultAddAsyncLogEntities.Where(ent => x == ent.Value).Count() > 0))
                .Returns(Task.Run(() => expectedResponse));
        }

        int _defaultAddCallCount = 0;
        public void DefaultAdd(
            LogEntity entity,
            int callCountForExpectedResponse,
            AzTableResponse expectedResponse,
            AzTableResponse defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<AzTableResponse>("DefaultAdd", ItExpr.Is<LogEntity>(x => x.CompareTo(entity)))
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
            AzTableResponse expectedResponse,
            AzTableResponse defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<Task<AzTableResponse>>("DefaultAddAsync", ItExpr.Is<LogEntity>(x => x.CompareTo(entity)), cancellationToken)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultAddAsyncCallCount)
                        return Task.Run(() => expectedResponse);

                    return Task.Run(() => defaultResponse);
                });
        }

        #endregion

        #region Protected get methods

        public void SetDefaultGet(
            string partitionKey,
            string rowKey,
            AzTableValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Protected().Setup<AzTableValueResponse<LogEntity>>("DefaultGet", true, partitionKey, rowKey)
                .Returns(expectedResponse);
        }

        public void SetDefaultGetAsync(
            string partitionKey,
            string rowKey,
            CancellationToken cancellationToken,
            AzTableValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Protected().Setup<Task<AzTableValueResponse<LogEntity>>>("DefaultGetAsync", true, partitionKey, rowKey, cancellationToken)
                .Returns(Task.Run(() => expectedResponse));
        }


        int _defaultGetCallCount = 0;
        public void DefaultGet(
            string partitionKey,
            string rowKey,
            int callCountForExpectedResponse,
            AzTableValueResponse<LogEntity> expectedResponse,
            AzTableValueResponse<LogEntity> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<AzTableValueResponse<LogEntity>>("DefaultGet", true, partitionKey, rowKey)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultGetCallCount)
                        return expectedResponse;

                    return defaultResponse;
                });
        }

        int _defaultGetAsyncCallCount = 0;
        public void DefaultGetAsync(
            string partitionKey,
            string rowKey,
            CancellationToken cancellationToken,
            int callCountForExpectedResponse,
            AzTableValueResponse<LogEntity> expectedResponse,
            AzTableValueResponse<LogEntity> defaultResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));
            AssertHelper.AssertNotNullOrThrow(defaultResponse, nameof(defaultResponse));

            _mock.Protected().Setup<Task<AzTableValueResponse<LogEntity>>>("DefaultGetAsync", true, partitionKey, rowKey, cancellationToken)
                .Returns(() =>
                {
                    if (callCountForExpectedResponse == ++_defaultGetAsyncCallCount)
                        return Task.Run(() => expectedResponse);

                    return Task.Run(() => defaultResponse);
                });
        }

        #endregion
    }
}
