
namespace Global.Logging.Tests
{
    public class AzLoggerAddToTableLogFilterTests : AzLoggerAddToTableBase
    {
        public AzLoggerAddToTableLogFilterTests()
        {
            _delegateContainer.GenerateLogTableNameDel = _generateConstantLogTableName;
            _delegateContainer.SetFillVerboseLabelSet(ServiceHelper.GenerateDefaultFillVerboseLabelSet);
        }

        #region Private methods

        private async Task AllowSeverityLevel_SuccessResponse_FailedResponse(
            IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> azSeverityLevelLogFilter,
            List<SeverityLevel> allowedSeverityLevels)
        {
            foreach (var severityLevel in _severityLevels)
            {
                var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                if (allowedSeverityLevels.Contains(severityLevel))
                {
                    // Act & Assert SuccessResponse
                    Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                        _constantLogTableName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter));

                    Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                        _constantLogTableName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter));
                }
                else
                {
                    // Act & Assert FailedResponse
                    Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                        _constantLogTableName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                        AzLoggerConstants.WritingNotAllowedMessageKey,
                        AzLoggerConstants.WritingNotAllowedMessage);

                    Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                        _constantLogTableName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                        AzLoggerConstants.WritingNotAllowedMessageKey,
                        AzLoggerConstants.WritingNotAllowedMessage);
                }

                Assert.Equal(severityLevel, entities.LogEntity.SeverityLevel);
            }
        }

        #endregion

        [Fact]
        public async Task AllowAnySeverityLevelForWriting_SuccessResponse()
        {
            foreach (var allowedSeverityLevel in _severityLevels)
            {
                // Arrange
                var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowSeverityLevelForWritingLogFilter(allowedSeverityLevel);

                var entities = EntityFactory.Create(severityLevel: allowedSeverityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                // Act & Assert SuccessResponse
                Assert.Equal(allowedSeverityLevel, entities.LogEntity.SeverityLevel);
                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                    _constantLogTableName,
                    entities,
                    delegateContainer: _delegateContainer,
                    azSeverityLevelLogFilter: azSeverityLevelLogFilter));

                Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                    _constantLogTableName,
                    entities,
                    delegateContainer: _delegateContainer,
                    azSeverityLevelLogFilter: azSeverityLevelLogFilter));
            }
        }

        [Fact]
        public async Task DenyAnySeverityLevelForWriting_FailedResponse()
        {
            foreach (var allowedSeverityLevel in _severityLevels)
            {
                // Arrange
                var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowSeverityLevelForWritingLogFilter(allowedSeverityLevel);

                foreach (var severityLevel in _severityLevels)
                {
                    if (severityLevel != allowedSeverityLevel)
                    {
                        // Arrange
                        var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                        // Act & Assert FailedResponse
                        Assert.NotEqual(allowedSeverityLevel, entities.LogEntity.SeverityLevel);
                        Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                            _constantLogTableName,
                            entities,
                            delegateContainer: _delegateContainer,
                            azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                            AzLoggerConstants.WritingNotAllowedMessageKey,
                            AzLoggerConstants.WritingNotAllowedMessage);

                        Asserts.FailedResponseWithMessage(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                            _constantLogTableName,
                            entities,
                            delegateContainer: _delegateContainer,
                            azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                            AzLoggerConstants.WritingNotAllowedMessageKey,
                            AzLoggerConstants.WritingNotAllowedMessage);
                    }
                }
            }
        }

        [Fact]
        public async Task AllowInfoSeverityLevelForWriting_SuccessResponse()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowInfoSeverityLevelForWritingLogFilter();

            // Act & Assert SuccessResponse
            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Sync, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer,
                azSeverityLevelLogFilter: azSeverityLevelLogFilter));

            Asserts.SuccessResponse(await CommonAddToTable(ConcurrentMethodType.Async, true, true,
                _constantLogTableName,
                delegateContainer: _delegateContainer,
                azSeverityLevelLogFilter: azSeverityLevelLogFilter));
        }

        [Fact]
        public async Task AllowLowSeverityLevelForWriting()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowLowSeverityLevelForWritingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _lowSeverityLevels);
        }

        [Fact]
        public async Task AllowHighSeverityLevelForWriting()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowHighSeverityLevelForWritingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _highSeverityLevels);
        }

        [Fact]
        public async Task AllowErrorSeverityLevelForWriting()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowErrorSeverityLevelForWritingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _errorSeverityLevels);
        }

        [Fact]
        public async Task AllowExceptionSeverityLevelForWriting()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowExceptionSeverityLevelForWritingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _exceptionSeverityLevels);
        }
    }
}
