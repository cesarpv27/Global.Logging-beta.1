
namespace Global.Logging.Tests
{
    public class AzLoggerTableWithRetriesTests : AzLoggerTableBase
    {
        bool _defaultCreateIfNotExists;
        int _callCountForExpectedResponse;

        public AzLoggerTableWithRetriesTests()
        {
            _callCountForExpectedResponse = 2;
        }

        [Fact]
        public async Task AddToTableWithRetries()
        {
            // Arrange
            _delegateContainer.SetGeneratePartitionKey(EntityHelper.GenerateConstantPartitionKey);
            _delegateContainer.SetGenerateRowKey(EntityHelper.GenerateConstantRowKey);
            _delegateContainer.FillVerboseLabelSetDel = ServiceHelper.GenerateConstantFillVerboseLabelSet;

            var entities = EntityFactory.Create(EntityHelper.GenerateConstantPartitionKey(), EntityHelper.GenerateConstantRowKey(),
                fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

            var tableName = ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
            _defaultCreateIfNotExists = true;

            CommonBuildAzTableRepositoryMock(ConcurrentMethodType.Sync, _defaultCreateIfNotExists, tableName);

            var azTableServiceMock = MockFactory.CreateAzTableServiceMock(_azTableRepositoryMock.MockObject);

            azTableServiceMock.DefaultAdd(
                entities.LogEntity,
                _callCountForExpectedResponse,
                AzTableResponseFactory.CreateSuccessful(),
                AzTableResponseFactory.CreateFailure());

            CommonBuildAzTableRepositoryMock(ConcurrentMethodType.Async, _defaultCreateIfNotExists, tableName);

            azTableServiceMock.DefaultAddAsync(
                entities.LogEntity,
                _defaultCancellationToken,
                _callCountForExpectedResponse,
                AzTableResponseFactory.CreateSuccessful(),
                AzTableResponseFactory.CreateFailure());

            var azLoggingService = ServiceFactory.CreateAzLoggingService(
                azTableServiceMock.MockObject,
                ServiceFactory.CreateAzBlobService(_azBlobRepositoryMock.MockObject));

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter, _delegateContainer);

            // Act
            var addToTableResponse = azLogger.AddToTable(entities.RawLog);
            var addToTableAsyncResponse = await azLogger.AddToTableAsync(entities.RawLog, cancellationToken: _defaultCancellationToken);

            // Assert
            Asserts.SuccessResponse(addToTableResponse);
            Asserts.SuccessResponse(addToTableAsyncResponse);
        }

        [Fact]
        public async Task GetFromTableWithRetries()
        {
            // Arrange
            var entities = EntityFactory.Create(EntityHelper.GenerateConstantPartitionKey(), EntityHelper.GenerateConstantRowKey(),
                fillVerboseLabelSet: ServiceHelper.GenerateConstantFillVerboseLabelSet);

            var partitionKey = entities.LogEntity.PartitionKey;
            var rowKey = entities.LogEntity.RowKey;

            var tableName = ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, partitionKey, rowKey);
            _defaultCreateIfNotExists = false;

            CommonBuildAzTableRepositoryMock(ConcurrentMethodType.Sync, _defaultCreateIfNotExists, tableName);

            var azTableServiceMock = MockFactory.CreateAzTableServiceMock(_azTableRepositoryMock.MockObject);

            azTableServiceMock.DefaultGet(
                partitionKey,
                rowKey,
                _callCountForExpectedResponse,
                AzTableValueResponseFactory.CreateSuccessful(entities.LogEntity),
                AzTableValueResponseFactory.CreateFailure<LogEntity>());

            CommonBuildAzTableRepositoryMock(ConcurrentMethodType.Async, _defaultCreateIfNotExists, tableName);

            azTableServiceMock.DefaultGetAsync(
                partitionKey,
                rowKey,
                _defaultCancellationToken,
                _callCountForExpectedResponse,
                AzTableValueResponseFactory.CreateSuccessful(entities.LogEntity),
                AzTableValueResponseFactory.CreateFailure<LogEntity>());

            var azLoggingService = ServiceFactory.CreateAzLoggingService(
                azTableServiceMock.MockObject,
                ServiceFactory.CreateAzBlobService(_azBlobRepositoryMock.MockObject));

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter);

            // Act
            var getFromTableResponse = azLogger.GetFromTable(tableName, partitionKey, rowKey);
            var getFromTableAsyncResponse = await azLogger.GetFromTableAsync(tableName, partitionKey, rowKey);

            // Assert
            LoggerAsserts.SuccessTableValueResponse(entities.LogEntity, getFromTableResponse, Extensions.CompareTo);
            LoggerAsserts.SuccessTableValueResponse(entities.LogEntity, getFromTableAsyncResponse, Extensions.CompareTo);
        }
    }
}
