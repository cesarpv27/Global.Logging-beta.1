
namespace Global.Logging.Tests
{
    public class AzLoggerAddToBlobTests : AzLoggerAddToBlobBase
    {
        public AzLoggerAddToBlobTests()
        {
            _delegateContainer.GenerateLogBlobContainerNameDel = _generateConstantLogBlobContainerName;
            _delegateContainer.GenerateLogBlobNameDel = _generateConstantLogBlobName;
        }

        [Fact]
        public async Task PropagateCreateBlobContainerIfNotExists()
        {
            // Assert createTableIfNotExists propagation
            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync, 
                _defaultOverwrite,
                true, true,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync, 
                _defaultOverwrite,
                false, false,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async, 
                _defaultOverwrite,
                true, true,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async, 
                _defaultOverwrite,
                false, false,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));

            // Act & Assert FailedResponse
            Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync, 
                _defaultOverwrite,
                true, false,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer),
                MockConstants.CreateIfNotExistsMessageKey,
                "False");

            Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async, 
                _defaultOverwrite,
                true, false,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer),
                MockConstants.CreateIfNotExistsMessageKey,
                "False");
        }

        [Fact]
        public async Task GenerateConstantLogBlobContainerNameConstantLogBlobName()
        {
            // Arrange Constant LogBlobContainerName, LogBlobName
            _delegateContainer.GenerateLogBlobContainerNameDel = _generateConstantLogBlobContainerName;
            _delegateContainer.GenerateLogBlobNameDel = _generateConstantLogBlobName;

            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer));
        }

        [Fact]
        public async Task DefaultBlobContainerNameBlobName_SetGenerateLogTableNameDel()
        {
            // Arrange default tableName
            _delegateContainer.SetGenerateLogBlobContainerName(null);// Using the default behavior
            _delegateContainer.SetGenerateLogBlobName(null);// Using the default behavior

            foreach (var severityLevel in _severityLevels)
            {
                var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                // Act & Assert SuccessResponse
                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    entities,
                    _delegateContainer));

                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async, 
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    entities,
                    _delegateContainer));

                // Act & Assert FailedResponse ConstantLogBlobContainerName
                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync, 
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, 
                    ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.BlobContainerNameMessageKey,
                    _constantLogBlobContainerName);

                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async, 
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, 
                    ServiceHelper.GenerateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    delegateContainer: _delegateContainer),
                    MockConstants.BlobContainerNameMessageKey,
                    _constantLogBlobContainerName);

                // Act & Assert FailedResponse ConstantLogBlobName
                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.BlobNameMessageKey,
                    _constantLogBlobName);

                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    ServiceHelper.GenerateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey),
                    _constantLogBlobName,
                    delegateContainer: _delegateContainer),
                    MockConstants.BlobNameMessageKey,
                    _constantLogBlobName);
            }
        }

        [Fact]
        public async Task NullRawLog()
        {
            // Arrange null RawLog
            var entities = EntityFactory.Create();
#pragma warning disable CS8625
            entities.RawLog = null;
#pragma warning restore CS8625

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(MockConstants.RawLogMessageKey, async () => await CommonAddToBlob(
                ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer));

            await Assert.ThrowsAsync<ArgumentNullException>(MockConstants.RawLogMessageKey, async () => await CommonAddToBlob(
                ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer));
        }

        [Fact]
        public async Task CustomPartitionKeyRowKey()
        {
            // Arrange with custom keys
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();

            var entities = EntityFactory.Create(partitionKey, rowKey, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);
            _delegateContainer.SetGeneratePartitionKey((entity) => partitionKey);
            _delegateContainer.SetGenerateRowKey((entity) => rowKey);

            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                _delegateContainer));

            // Arrange
            _delegateContainer.SetGeneratePartitionKey(null);
            _delegateContainer.SetGenerateRowKey(null);

            // Act & Assert FailedResponse
            var response = await CommonAddToBlob(ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityPartitionKeyMessageKey,
                partitionKey);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityRowKeyMessageKey,
                rowKey);

            response = await CommonAddToBlob(ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityPartitionKeyMessageKey,
                partitionKey);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityRowKeyMessageKey,
                rowKey);
        }

        [Fact]
        public async Task NullPartitionKey()
        {
            // Arrange with null partitionKey
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();

            var entities = EntityFactory.Create(partitionKey, rowKey, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

#pragma warning disable CS8600
            partitionKey = null;
#pragma warning restore

#pragma warning disable CS8603
            _delegateContainer.SetGeneratePartitionKey((entity) => partitionKey);
#pragma warning restore

            _delegateContainer.SetGenerateRowKey((entity) => rowKey);

            // Act & Assert
            Asserts.FailedResponse<AzBlobResponse, ArgumentNullException>(await CommonAddToBlob(
                ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.PartitionKeyMessageKey);

            Asserts.FailedResponse<AzBlobResponse, ArgumentNullException>(await CommonAddToBlob(
                ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.PartitionKeyMessageKey);
        }

        [Fact]
        public async Task NullRowKey()
        {
            // Arrange with null rowKey
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();

            var entities = EntityFactory.Create(partitionKey, rowKey, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

#pragma warning disable CS8600
            rowKey = null;
#pragma warning restore

#pragma warning disable CS8603
            _delegateContainer.SetGenerateRowKey((entity) => rowKey);
#pragma warning restore

            _delegateContainer.SetGeneratePartitionKey((entity) => partitionKey);

            // Act & Assert
            Asserts.FailedResponse<AzBlobResponse, ArgumentNullException>(await CommonAddToBlob(
                ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.RowKeyMessageKey);

            Asserts.FailedResponse<AzBlobResponse, ArgumentNullException>(await CommonAddToBlob(
                ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.RowKeyMessageKey);
        }

        [Fact]
        public async Task SetFillVerboseLabelSetDel()
        {
            foreach (var severityLevel in _severityLevels)
            {
                // Arrange
                _delegateContainer.SetFillVerboseLabelSet(ServiceHelper.GenerateDefaultFillVerboseLabelSet);
                var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: ServiceHelper.GenerateDefaultFillVerboseLabelSet);

                // Act & Assert SuccessResponse
                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
                    entities,
                    _delegateContainer));

                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
                    entities,
                    _delegateContainer));

                // Act & Assert FailedResponse
                _delegateContainer.SetFillVerboseLabelSet(ServiceHelper.GenerateConstantFillVerboseLabelSet);
                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.EntityPartitionKeyMessageKey,
                    entities.LogEntity.PartitionKey);

                Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.EntityPartitionKeyMessageKey,
                    entities.LogEntity.PartitionKey);
            }
        }
    }
}
