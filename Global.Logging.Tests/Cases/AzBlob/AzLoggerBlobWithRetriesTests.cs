
namespace Global.Logging.Tests
{
    public class AzLoggerBlobWithRetriesTests : AzLoggerBlobBase
    {
        bool _defaultCreateIfNotExists;
        int _callCountForExpectedResponse;

        public AzLoggerBlobWithRetriesTests()
        {
            _callCountForExpectedResponse = 2;
        }

        [Fact]
        public async Task AddToBlobWithRetries()
        {
            // Arrange
            _delegateContainer.SetGeneratePartitionKey(EntityHelper.GenerateConstantPartitionKey);
            _delegateContainer.SetGenerateRowKey(EntityHelper.GenerateConstantRowKey);
            _delegateContainer.FillVerboseLabelSetDel = ServiceHelper.GenerateConstantFillVerboseLabelSet;

            var entities = EntityFactory.Create(EntityHelper.GenerateConstantPartitionKey(), EntityHelper.GenerateConstantRowKey(),
                fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

            string logBlobContainerName = ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
            string logBlobName = ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

            _defaultCreateIfNotExists = true;

            CommonBuildAzBlobRepositoryMock(ConcurrentMethodType.Sync, _defaultCreateIfNotExists, logBlobContainerName, logBlobName);

            var azBlobServiceMock = MockFactory.CreateAzBlobServiceMock(_azBlobRepositoryMock.MockObject);

            azBlobServiceMock.DefaultAdd(
                entities.LogEntity,
                _callCountForExpectedResponse,
                AzBlobValueResponseFactory.CreateSuccessful(RepositoryHelper.CreateDefaultBlobContentInfo()),
                AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>());

            CommonBuildAzBlobRepositoryMock(ConcurrentMethodType.Async, _defaultCreateIfNotExists, logBlobContainerName, logBlobName);

            azBlobServiceMock.DefaultAddAsync(
                entities.LogEntity,
                _defaultCancellationToken,
                _callCountForExpectedResponse,
                AzBlobValueResponseFactory.CreateSuccessful(RepositoryHelper.CreateDefaultBlobContentInfo()),
                AzBlobValueResponseFactory.CreateFailure<BlobContentInfo>());

            var azLoggingService = ServiceFactory.CreateAzLoggingService(
                ServiceFactory.CreateAzTableService(_azTableRepositoryMock.MockObject),
                azBlobServiceMock.MockObject);

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter, _delegateContainer);

            // Act
            var addToBlobResponse = azLogger.AddToBlob(entities.RawLog);
            var addToBlobAsyncResponse = await azLogger.AddToBlobAsync(entities.RawLog, cancellationToken: _defaultCancellationToken);

            // Assert
            Asserts.SuccessResponse(addToBlobResponse);
            Asserts.SuccessResponse(addToBlobAsyncResponse);
        }

        [Fact]
        public async Task GetFromBlobWithRetries()
        {
            // Arrange
            var entities = EntityFactory.Create(EntityHelper.GenerateConstantPartitionKey(), EntityHelper.GenerateConstantRowKey(),
                fillVerboseLabelSet: ServiceHelper.GenerateConstantFillVerboseLabelSet);

            var partitionKey = entities.LogEntity.PartitionKey;
            var rowKey = entities.LogEntity.RowKey;

            string logBlobContainerName = ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
            string logBlobName = ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

            _defaultCreateIfNotExists = false;

            CommonBuildAzBlobRepositoryMock(ConcurrentMethodType.Sync, _defaultCreateIfNotExists, logBlobContainerName, logBlobName);

            var azBlobServiceMock = MockFactory.CreateAzBlobServiceMock(_azBlobRepositoryMock.MockObject);

            azBlobServiceMock.DefaultGet(
                _callCountForExpectedResponse,
                AzBlobValueResponseFactory.CreateSuccessful(entities.LogEntity),
                AzBlobValueResponseFactory.CreateFailure<LogEntity>());

            CommonBuildAzBlobRepositoryMock(ConcurrentMethodType.Async, _defaultCreateIfNotExists, logBlobContainerName, logBlobName);

            azBlobServiceMock.DefaultGetAsync(
                _defaultCancellationToken,
                _callCountForExpectedResponse,
                AzBlobValueResponseFactory.CreateSuccessful(entities.LogEntity),
                AzBlobValueResponseFactory.CreateFailure<LogEntity>());

            var azLoggingService = ServiceFactory.CreateAzLoggingService(
                ServiceFactory.CreateAzTableService(_azTableRepositoryMock.MockObject),
                azBlobServiceMock.MockObject);

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter);

            // Act
            var getFromBlobResponse = azLogger.GetFromBlob(logBlobContainerName, logBlobName);
            var getFromBlobAsyncResponse = await azLogger.GetFromBlobAsync(logBlobContainerName, logBlobName);

            // Assert
            LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, getFromBlobResponse, Extensions.CompareTo);
            LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, getFromBlobAsyncResponse, Extensions.CompareTo);
        }
    }
}
