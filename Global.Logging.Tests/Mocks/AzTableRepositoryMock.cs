
namespace Global.Logging.Tests
{
    public sealed class AzTableRepositoryMock
    {
        private Mock<IAzTableRepository<LogEntity>> _mock;

        public IAzTableRepository<LogEntity> MockObject { get { return _mock.Object; } }

        public AzTableRepositoryMock()
        {
            _mock = new Mock<IAzTableRepository<LogEntity>>();
        }

        private AzTableResponse GetResponseOrCreateSuccessful(AzTableResponse? expectedResponse)
        {
            if (expectedResponse != default)
                return expectedResponse;
            return AzTableResponseFactory.CreateSuccessful();
        }

        public void SetLoadTableClient(
            string expectedTableName,
            bool expectedCreateIfNotExists,
            AzTableResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadTableClient(expectedTableName, expectedCreateIfNotExists))
                .Returns(expectedResponse);
        }

        public void SetLoadTableClientAsync(
            string expectedTableName,
            bool expectedCreateIfNotExists,
            CancellationToken expectedCancellationToken,
            AzTableResponse expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.LoadTableClientAsync(expectedTableName, expectedCreateIfNotExists, expectedCancellationToken))
                .Returns(Task.Run(() => expectedResponse));
        }

        public void LoadTableClient(
            string expectedTableName, 
            bool expectedCreateIfNotExists,
            AzTableResponse? expectedResponse = default)
        {
            _mock.Setup(x => x.LoadTableClient(
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .Returns((
                    string inTableName,
                    bool inCreateIfNotExists) =>
                {
                    var response = AzTableResponseFactory.CreateFailure();
                    if (!string.Equals(inTableName, expectedTableName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.TableNameMessageKey, expectedTableName);
                    else if (inCreateIfNotExists != expectedCreateIfNotExists)
                        response.AddMessageTransactionally(MockConstants.CreateIfNotExistsMessageKey, expectedCreateIfNotExists.ToString());
                    else
                        return GetResponseOrCreateSuccessful(expectedResponse);

                    return response;
                });
        }

        public void LoadTableClientAsync(
            string expectedTableName,
            bool expectedCreateIfNotExists,
            CancellationToken expectedCancellationToken,
            AzTableResponse? expectedResponse = default)
        {
            _mock.Setup(x => x.LoadTableClientAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()))
                .Returns((
                    string inTableName,
                    bool inCreateIfNotExists,
                    CancellationToken inCancellationToken) =>
                {
                    var response = AzTableResponseFactory.CreateFailure();
                    if (!string.Equals(inTableName, expectedTableName, StringComparison.InvariantCulture))
                        response.AddMessageTransactionally(MockConstants.TableNameMessageKey, expectedTableName);
                    else if (inCreateIfNotExists != expectedCreateIfNotExists)
                        response.AddMessageTransactionally(MockConstants.CreateIfNotExistsMessageKey, expectedCreateIfNotExists.ToString());
                    else if (inCancellationToken != expectedCancellationToken)
                        response.AddMessageTransactionally(MockConstants.CancellationTokenMessageKey, MockConstants.CancellationTokenMessageKey);
                    else
                        return Task.Run(() => GetResponseOrCreateSuccessful(expectedResponse));

                    return Task.Run(() => response);
                });
        }

        public void Add(
            LogEntity expectedEntity,
            AzTableResponse? expectedResponse = default)
        {
            _mock.Setup(x => x.Add(
                It.IsAny<LogEntity>()))
                .Returns((LogEntity inEntity) =>
                {
                    var response = AzTableResponseFactory.CreateFailure();
                    if (!inEntity.CompareTo(expectedEntity))
                    {
                        response.AddMessageTransactionally(MockConstants.EntityPartitionKeyMessageKey, expectedEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.EntityRowKeyMessageKey, expectedEntity.RowKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityPartitionKeyMessageKey, inEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityRowKeyMessageKey, inEntity.RowKey);
                    }
                    else
                        return GetResponseOrCreateSuccessful(expectedResponse);

                    return response;
                });
        }

        public void AddAsync(
            LogEntity expectedEntity,
            CancellationToken expectedCancellationToken,
            AzTableResponse? expectedResponse = default)
        {
            _mock.Setup(x => x.AddAsync(
                It.IsAny<LogEntity>(),
                It.IsAny<CancellationToken>()))
                .Returns((
                    LogEntity inEntity,
                    CancellationToken inCancellationToken) =>
                {
                    var response = AzTableResponseFactory.CreateFailure();
                    if (!inEntity.CompareTo(expectedEntity))
                    {
                        response.AddMessageTransactionally(MockConstants.EntityPartitionKeyMessageKey, expectedEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.EntityRowKeyMessageKey, expectedEntity.RowKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityPartitionKeyMessageKey, inEntity.PartitionKey);
                        response.AddMessageTransactionally(MockConstants.MockedEntityRowKeyMessageKey, inEntity.RowKey);
                    }
                    else if (inCancellationToken != expectedCancellationToken)
                        response.AddMessageTransactionally(MockConstants.CancellationTokenMessageKey, MockConstants.CancellationTokenMessageKey);
                    else
                        return Task.Run(() => GetResponseOrCreateSuccessful(expectedResponse));

                    return Task.Run(() => response);
                });
        }

        public void Get(
            string expectedPartitionKey, 
            string expectedRowKey,
            AzTableValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.Get(
                It.Is<string>(pKey => string.Equals(pKey, expectedPartitionKey, StringComparison.InvariantCulture)),
                It.Is<string>(rKey => string.Equals(rKey, expectedRowKey, StringComparison.InvariantCulture))))
                .Returns(expectedResponse);
        }

        public void GetAsync(
            string expectedPartitionKey, 
            string expectedRowKey, 
            CancellationToken expectedCancellationToken,
            AzTableValueResponse<LogEntity> expectedResponse)
        {
            AssertHelper.AssertNotNullOrThrow(expectedResponse, nameof(expectedResponse));

            _mock.Setup(x => x.GetAsync(
                It.Is<string>(pKey => string.Equals(pKey, expectedPartitionKey, StringComparison.InvariantCulture)),
                It.Is<string>(rKey => string.Equals(rKey, expectedRowKey, StringComparison.InvariantCulture)),
                It.Is<CancellationToken>(x => x == expectedCancellationToken)))
                .Returns(Task.Run(() => expectedResponse));
        }
    }
}
