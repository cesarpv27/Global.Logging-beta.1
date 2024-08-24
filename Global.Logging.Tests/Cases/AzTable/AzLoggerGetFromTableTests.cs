
namespace Global.Logging.Tests
{
    public class AzLoggerGetFromTableTests : AzLoggerGetFromTableBase
    {
        [Fact]
        public async Task GetFromTable()
        {
            // Act
            var syncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName, _constantLogTableName,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName, _constantLogTableName,
                delegateContainer: _delegateContainer);

            // Act & Assert
            LoggerAsserts.SuccessTableValueResponse(syncGetFromResponse.ExpectedLogEntity, syncGetFromResponse.ActualResponse, Extensions.CompareTo);
            LoggerAsserts.SuccessTableValueResponse(asyncGetFromResponse.ExpectedLogEntity, asyncGetFromResponse.ActualResponse, Extensions.CompareTo);
        }

        [Fact]
        public async Task NullPartitionKey()
        {
            // Arrange with null partitionKey
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();

            var entities = EntityFactory.Create(partitionKey, rowKey);

#pragma warning disable CS8625
            entities.LogEntity.PartitionKey = null;
#pragma warning restore

            // Act
            var syncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName, _constantLogTableName, partitionKey, rowKey, entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName, _constantLogTableName, partitionKey, rowKey, entities,
                delegateContainer: _delegateContainer);

            // Assert
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentNullException>(syncGetFromResponse.ActualResponse, MockConstants.PartitionKeyMessageKey);
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentNullException>(asyncGetFromResponse.ActualResponse, MockConstants.PartitionKeyMessageKey);
        }

        [Fact]
        public async Task NullRowKey()
        {
            // Arrange with null rowKey
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();

            var entities = EntityFactory.Create(partitionKey, rowKey);

#pragma warning disable CS8625
            entities.LogEntity.RowKey = null;
#pragma warning restore

            // Act
            var syncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName, _constantLogTableName, partitionKey, rowKey, entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName, _constantLogTableName, partitionKey, rowKey, entities,
                delegateContainer: _delegateContainer);

            // Assert
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentNullException>(syncGetFromResponse.ActualResponse, MockConstants.RowKeyMessageKey);
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentNullException>(asyncGetFromResponse.ActualResponse, MockConstants.RowKeyMessageKey);
        }

        [Fact]
        public async Task NullTableName()
        {
            // Arrange with null tableName
            var entities = EntityFactory.Create();
            var expectedTableName = ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625
            // Act
            var syncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Sync, true, true,
                expectedTableName, default, entities: entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Async, true, true,
                expectedTableName, default, entities: entities,
                delegateContainer: _delegateContainer);
#pragma warning restore

            // Assert
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentException>(syncGetFromResponse.ActualResponse, MockConstants.TableNameMessageKey);
            Asserts.FailedResponse<AzTableValueResponse<ReadOnlyLog>, ArgumentException>(asyncGetFromResponse.ActualResponse, MockConstants.TableNameMessageKey);
        }

        [Fact]
        public async Task DiferentTableName()
        {
            // Arrange with null tableName
            var entities = EntityFactory.Create();
            var expectedTableName = ServiceHelper.GenerateDefaultLogTableName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625
            // Act
            var syncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Sync, true, true,
                expectedTableName, _constantLogTableName, entities: entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromTable(ConcurrentMethodType.Async, true, true,
                expectedTableName, _constantLogTableName, entities: entities,
                delegateContainer: _delegateContainer);
#pragma warning restore

            // Assert
            Asserts.FailedResponseWithMessage(syncGetFromResponse.ActualResponse, MockConstants.TableNameMessageKey, expectedTableName);
            Asserts.FailedResponseWithMessage(asyncGetFromResponse.ActualResponse, MockConstants.TableNameMessageKey, expectedTableName);
        }
    }
}
