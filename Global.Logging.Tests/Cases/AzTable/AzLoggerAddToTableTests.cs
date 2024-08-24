
namespace Global.Logging.Tests
{
    public class AzLoggerAddToTableTests : AzLoggerAddToTableBase
    {
        public AzLoggerAddToTableTests()
        {
            _delegateContainer.GenerateLogTableNameDel = _generateConstantLogTableName;
        }

        [Fact]
        public async Task PropagateCreateTableIfNotExists()
        {
            // Assert createTableIfNotExists propagation
            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer));
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, false, false,
                _constantLogTableName,
                delegateContainer: _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer));
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, false, false,
                _constantLogTableName,
                delegateContainer: _delegateContainer));

            // Act & Assert FailedResponse
            Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Sync, true, false,
                _constantLogTableName,
                delegateContainer: _delegateContainer),
                MockConstants.CreateIfNotExistsMessageKey,
                "False");

            Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Async, true, false,
                _constantLogTableName,
                delegateContainer: _delegateContainer),
                MockConstants.CreateIfNotExistsMessageKey,
                "False");
        }

        [Fact]
        public async Task GenerateConstantTableName()
        {
            // Arrange Constant tableName
            _delegateContainer.GenerateLogTableNameDel = _generateConstantLogTableName;

            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer));
        }

        [Fact]
        public async Task DefaultTableName_SetGenerateLogTableNameDel()
        {
            // Arrange default tableName
            _delegateContainer.SetGenerateLogTableName(null);// Using the default behavior

            foreach (var severityLevel in _severityLevels)
            {
                var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                // Act & Assert SuccessResponse
                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                    ServiceHelper.GenerateDefaultLogTableName(severityLevel),
                    entities,
                    _delegateContainer));

                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                    ServiceHelper.GenerateDefaultLogTableName(severityLevel),
                    entities,
                    _delegateContainer));

                // Act & Assert FailedResponse
                Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                    _constantLogTableName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.TableNameMessageKey,
                    _constantLogTableName);

                Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                    _constantLogTableName,
                    delegateContainer: _delegateContainer),
                    MockConstants.TableNameMessageKey,
                    _constantLogTableName);
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
            await Assert.ThrowsAsync<ArgumentNullException>(MockConstants.RawLogMessageKey, async () => await CommonAddToTable(
                ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                entities,
                delegateContainer: _delegateContainer));

            await Assert.ThrowsAsync<ArgumentNullException>(MockConstants.RawLogMessageKey, async () => await CommonAddToTable(
                ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
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
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                entities,
                _delegateContainer));

            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
                entities,
                _delegateContainer));

            // Arrange
            _delegateContainer.SetGeneratePartitionKey(null);
            _delegateContainer.SetGenerateRowKey(null);

            // Act & Assert FailedResponse
            var response = await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                entities,
                delegateContainer: _delegateContainer);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityPartitionKeyMessageKey,
                partitionKey);
            Asserts.FailedResponseWithMessage(response,
                MockConstants.EntityRowKeyMessageKey,
                rowKey);

            response = await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
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
            Asserts.FailedResponse<AzTableResponse, ArgumentNullException>(await CommonAddToTable(
                ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.PartitionKeyMessageKey);

            Asserts.FailedResponse<AzTableResponse, ArgumentNullException>(await CommonAddToTable(
                ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
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
            Asserts.FailedResponse<AzTableResponse, ArgumentNullException>(await CommonAddToTable(
                ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                entities,
                delegateContainer: _delegateContainer), MockConstants.RowKeyMessageKey);

            Asserts.FailedResponse<AzTableResponse, ArgumentNullException>(await CommonAddToTable(
                ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
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
                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true, 
                    _constantLogTableName,
                    entities,
                    _delegateContainer));

                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                    _constantLogTableName,
                    entities,
                    _delegateContainer));

                // Act & Assert FailedResponse
                _delegateContainer.SetFillVerboseLabelSet(ServiceHelper.GenerateConstantFillVerboseLabelSet);
                Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                    _constantLogTableName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.EntityPartitionKeyMessageKey,
                    entities.LogEntity.PartitionKey);

                Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                    _constantLogTableName,
                    entities,
                    delegateContainer: _delegateContainer),
                    MockConstants.EntityPartitionKeyMessageKey,
                    entities.LogEntity.PartitionKey);
            }
        }
    }
}
