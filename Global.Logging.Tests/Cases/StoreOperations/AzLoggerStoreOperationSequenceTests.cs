
namespace Global.Logging.Tests
{
    public class AzLoggerStoreOperationSequenceTests : AzLoggerBase
    {
        protected readonly string _constantPartitionKey;
        protected readonly string _constantRowKey;
        protected readonly int _defaultCallCountForExpectedResponse;
        protected bool _createTableIfNotExists;
        protected bool _createBlobContainerIfNotExists;

        public AzLoggerStoreOperationSequenceTests()
        {
            _delegateContainer.FillVerboseLabelSetDel = ServiceHelper.GenerateConstantFillVerboseLabelSet;

            _retryOnFailures = false;
            _maxRetryAttempts = 1;

            _constantPartitionKey = EntityHelper.GenerateConstantPartitionKey();
            _constantRowKey = EntityHelper.GenerateConstantRowKey();

            _defaultCallCountForExpectedResponse = 1;
            _createTableIfNotExists = true;
            _createBlobContainerIfNotExists = true;
        }

        protected void BuildLoadsInAzTableRepositoryMock(
            AzTableRepositoryMock azTableRepositoryMock,
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            AzTableResponse? expectedLoadTableClientResponse = null,
            CancellationToken? loadTableClientAsyncCancellationToken = null)
        {
            expectedLoadTableClientResponse ??= AzTableResponseFactory.CreateSuccessful();
            loadTableClientAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
                azTableRepositoryMock.SetLoadTableClient(expectedTableName, expectedCreateTableIfNotExists, expectedLoadTableClientResponse);
            else
                azTableRepositoryMock.SetLoadTableClientAsync(expectedTableName, expectedCreateTableIfNotExists, loadTableClientAsyncCancellationToken.Value, expectedLoadTableClientResponse);
        }

        protected void BuildLoadsInAzBlobRepositoryMock(
            AzBlobRepositoryMock azBlobRepositoryMock,
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string expectedLogBlobName,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null,
            CancellationToken? loadBlobContainerClientAsyncCancellationToken = null,
            CancellationToken? loadBlobClientAsyncCancellationToken = null)
        {
            expectedLoadBlobContainerClientResponse ??= AzBlobResponseFactory.CreateSuccessful();
            expectedLoadBlobClientResponse ??= AzBlobResponseFactory.CreateSuccessful();

            loadBlobContainerClientAsyncCancellationToken ??= _defaultCancellationToken;
            loadBlobClientAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
            {
                azBlobRepositoryMock.SetLoadBlobContainerClient(expectedLogBlobContainerName, expectedCreateBlobContainerIfNotExists, expectedLoadBlobContainerClientResponse);
                azBlobRepositoryMock.SetLoadBlobClient(expectedLogBlobName, expectedLoadBlobClientResponse);
            }
            else
            {
                azBlobRepositoryMock.SetLoadBlobContainerClientAsync(expectedLogBlobContainerName, expectedCreateBlobContainerIfNotExists,
                   loadBlobContainerClientAsyncCancellationToken.Value, expectedLoadBlobContainerClientResponse);
                azBlobRepositoryMock.SetLoadBlobClientAsync(expectedLogBlobName, loadBlobClientAsyncCancellationToken.Value, expectedLoadBlobClientResponse);
            }
        }

        protected AzLogger BuildAzLogger(
            List<(RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)> list)
        {
            // Arrange
            var azTableRepositoryMock = MockFactory.CreateAzTableRepositoryMock();
            var azTableServiceMock = MockFactory.CreateAzTableServiceMock(azTableRepositoryMock.MockObject);

            var azBlobRepositoryMock = MockFactory.CreateAzBlobRepositoryMock();
            var azBlobServiceMock = MockFactory.CreateAzBlobServiceMock(azBlobRepositoryMock.MockObject);

            string tableName;
            string logBlobContainerName;
            string logBlobName;
            foreach (var entities in list)
            {
                tableName = ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
                BuildLoadsInAzTableRepositoryMock(azTableRepositoryMock, ConcurrentMethodType.Sync, _createTableIfNotExists, tableName);
                BuildLoadsInAzTableRepositoryMock(azTableRepositoryMock, ConcurrentMethodType.Async, _createTableIfNotExists, tableName);

                logBlobContainerName = ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
                logBlobName = ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
                BuildLoadsInAzBlobRepositoryMock(azBlobRepositoryMock, ConcurrentMethodType.Sync, _createBlobContainerIfNotExists, logBlobContainerName, logBlobName);
                BuildLoadsInAzBlobRepositoryMock(azBlobRepositoryMock, ConcurrentMethodType.Async, _createBlobContainerIfNotExists, logBlobContainerName, logBlobName);

                // azTableServiceMock
                azTableServiceMock.SetDefaultAdd(
                    entities.LogEntity,
                    AzTableResponseFactory.CreateSuccessful());

                azTableServiceMock.SetDefaultAddAsync(
                    entities.LogEntity,
                    _defaultCancellationToken,
                    AzTableResponseFactory.CreateSuccessful());

                azTableServiceMock.SetDefaultGet(
                    entities.LogEntity.PartitionKey,
                    entities.LogEntity.RowKey,
                    AzTableValueResponseFactory.CreateSuccessful(entities.LogEntity));

                azTableServiceMock.SetDefaultGetAsync(
                    entities.LogEntity.PartitionKey,
                    entities.LogEntity.RowKey,
                    _defaultCancellationToken,
                    AzTableValueResponseFactory.CreateSuccessful(entities.LogEntity));

                // azBlobServiceMock
                azBlobServiceMock.SetDefaultAdd(
                    entities.LogEntity,
                    AzBlobValueResponseFactory.CreateSuccessful(RepositoryHelper.CreateDefaultBlobContentInfo()));

                azBlobServiceMock.SetDefaultAddAsync(
                    entities.LogEntity,
                    _defaultCancellationToken,
                    AzBlobValueResponseFactory.CreateSuccessful(RepositoryHelper.CreateDefaultBlobContentInfo()));

                azBlobServiceMock.Set_GetWithRetry(
                    logBlobContainerName,
                    logBlobName,
                    _maxRetryAttempts!.Value,
                    _createBlobContainerIfNotExists,
                    AzBlobValueResponseFactory.CreateSuccessful(entities.LogEntity));

                azBlobServiceMock.Set_GetWithRetryAsync(
                    logBlobContainerName,
                    logBlobName,
                    _maxRetryAttempts!.Value,
                    _createBlobContainerIfNotExists,
                    _defaultCancellationToken,
                    AzBlobValueResponseFactory.CreateSuccessful(entities.LogEntity));

            }

            var azLoggingService = ServiceFactory.CreateAzLoggingService(
                azTableServiceMock.MockObject,
                azBlobServiceMock.MockObject);

            return ModelFactory.CreateAzLogger(azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter, _delegateContainer);
        }

        [Fact]
        public async Task NextAlwaysNextOnComplete()
        {
            // Arrange
            List<(RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)> list = new(_severityLevels.Count);
            foreach (var severityLevel in _severityLevels)
                list.Add(EntityFactory.Create(_constantPartitionKey, _constantRowKey, severityLevel: severityLevel,
                    fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel));

            var storeOperationSequence = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();
            foreach (var sequenceExecutionType in new[] { SequenceExecutionType.NextAlways, SequenceExecutionType.NextOnComplete })
            {
                foreach (var entities in list)
                {
                    storeOperationSequence.AddNext(
                        new AddToTableOperation<RawLog<Exception>, Exception>(
                            entities.RawLog,
                            sequenceExecutionType,
                            _retryOnFailures!.Value,
                            _maxRetryAttempts,
                            _createTableIfNotExists));

                    storeOperationSequence.AddNext(
                        new GetFromTableOperation<ReadOnlyLog>(
                            ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                            _constantPartitionKey,
                            _constantRowKey,
                            sequenceExecutionType,
                            _retryOnFailures!.Value,
                            _maxRetryAttempts,
                            _createTableIfNotExists));

                    storeOperationSequence.AddNext(
                        new AddToBlobOperation<RawLog<Exception>, Exception>(
                            entities.RawLog,
                            sequenceExecutionType,
                            _retryOnFailures!.Value,
                            _maxRetryAttempts,
                            _createBlobContainerIfNotExists));

                    storeOperationSequence.AddNext(
                        new GetFromBlobOperation<ReadOnlyLog>(
                            ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                            ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                            sequenceExecutionType,
                            _retryOnFailures!.Value,
                            _maxRetryAttempts,
                            _createBlobContainerIfNotExists));
                }
            }

            var azLogger = BuildAzLogger(list);

            // Act sync
            azLogger.ExecuteSequence(storeOperationSequence);

            // Assert sync
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            // Act async
            await azLogger.ExecuteSequenceAsync(storeOperationSequence, _defaultCancellationToken);

            // Assert async
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Asserts.SuccessResponse(operation.Response);
        }

        [Fact]
        public async Task NextAlwaysNextNever()
        {
            // Arrange
            List<(RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)> list = new(_severityLevels.Count);
            foreach (var severityLevel in _severityLevels)
                list.Add(EntityFactory.Create(_constantPartitionKey, _constantRowKey, severityLevel: severityLevel,
                    fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel));

            var storeOperationSequence = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();
            foreach (var entities in list)
            {
                storeOperationSequence.AddNext(
                    new AddToTableOperation<RawLog<Exception>, Exception>(
                        entities.RawLog,
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new GetFromTableOperation<ReadOnlyLog>(
                        ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        _constantPartitionKey,
                        _constantRowKey,
                        SequenceExecutionType.NextNever,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new AddToBlobOperation<RawLog<Exception>, Exception>(
                        entities.RawLog,
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createBlobContainerIfNotExists));
            }

            var azLogger = BuildAzLogger(list);

            // Act sync
            azLogger.ExecuteSequence(storeOperationSequence);

            // Assert sync
            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            Asserts.SuccessResponse(storeOperationSequence.EnumerateGetFromTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Assert.Null(operation.Response);

            // Act async
            await azLogger.ExecuteSequenceAsync(storeOperationSequence, _defaultCancellationToken);

            // Assert async
            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            Asserts.SuccessResponse(storeOperationSequence.EnumerateGetFromTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Assert.Null(operation.Response);
        }

        [Fact]
        public async Task NextOnFails_ShouldExecuteNext()
        {
            // Arrange
            List<(RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)> list = new(_severityLevels.Count);
            foreach (var severityLevel in _severityLevels)
                list.Add(EntityFactory.Create(_constantPartitionKey, _constantRowKey, severityLevel: severityLevel,
                    fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel));

            var testRawLog = new RawLog<Exception>("test source", SeverityLevel.Info, string.Empty);

            var storeOperationSequence = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();
            foreach (var entities in list)
            {
                storeOperationSequence.AddNext(
                    new AddToTableOperation<RawLog<Exception>, Exception>(
                        testRawLog,
                        SequenceExecutionType.NextOnFails,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new AddToBlobOperation<RawLog<Exception>, Exception>(
                        entities.RawLog,
                        SequenceExecutionType.NextNever,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createBlobContainerIfNotExists));

                storeOperationSequence.AddNext(
                    new GetFromTableOperation<ReadOnlyLog>(
                        ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        _constantPartitionKey,
                        _constantRowKey,
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new GetFromBlobOperation<ReadOnlyLog>(
                        ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createBlobContainerIfNotExists));
            }

            var azLogger = BuildAzLogger(list);

            // Act sync
            azLogger.ExecuteSequence(storeOperationSequence);

            // Assert sync
            Asserts.FailedResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToBlobOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Assert.Null(operation.Response);

            // Act async
            await azLogger.ExecuteSequenceAsync(storeOperationSequence, _defaultCancellationToken);

            // Assert async
            Asserts.FailedResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToBlobOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Assert.Null(operation.Response);
        }

        [Fact]
        public async Task NextOnFails_ShouldNotExecuteNext()
        {
            // Arrange
            List<(RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)> list = new(_severityLevels.Count);
            foreach (var severityLevel in _severityLevels)
                list.Add(EntityFactory.Create(_constantPartitionKey, _constantRowKey, severityLevel: severityLevel,
                    fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel));

            var storeOperationSequence = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();
            foreach (var entities in list)
            {
                storeOperationSequence.AddNext(
                    new AddToTableOperation<RawLog<Exception>, Exception>(
                        entities.RawLog,
                        SequenceExecutionType.NextOnFails,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new AddToBlobOperation<RawLog<Exception>, Exception>(
                        entities.RawLog,
                        SequenceExecutionType.NextNever,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createBlobContainerIfNotExists));

                storeOperationSequence.AddNext(
                    new GetFromTableOperation<ReadOnlyLog>(
                        ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        _constantPartitionKey,
                        _constantRowKey,
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createTableIfNotExists));

                storeOperationSequence.AddNext(
                    new GetFromBlobOperation<ReadOnlyLog>(
                        ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                        SequenceExecutionType.NextAlways,
                        _retryOnFailures!.Value,
                        _maxRetryAttempts,
                        _createBlobContainerIfNotExists));
            }

            var azLogger = BuildAzLogger(list);

            // Act sync
            azLogger.ExecuteSequence(storeOperationSequence);

            // Assert sync
            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Assert.Null(operation.Response);

            // Act async
            await azLogger.ExecuteSequenceAsync(storeOperationSequence, _defaultCancellationToken);

            // Assert async
            Asserts.SuccessResponse(storeOperationSequence.EnumerateAddToTableOperations().First().Response);
            foreach (var operation in storeOperationSequence.EnumerateAddToTableOperations().Skip(1))
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateAddToBlobOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromTableOperations())
                Assert.Null(operation.Response);

            foreach (var operation in storeOperationSequence.EnumerateGetFromBlobOperations())
                Assert.Null(operation.Response);
        }
    }
}
