
namespace Global.Logging.Tests
{
    public class AzLoggerAddToBlobLogFilterTests : AzLoggerAddToBlobBase
    {
        public AzLoggerAddToBlobLogFilterTests()
        {
            _delegateContainer.GenerateLogBlobContainerNameDel = _generateConstantLogBlobContainerName;
            _delegateContainer.GenerateLogBlobNameDel = _generateConstantLogBlobName;
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
                    Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                        _defaultOverwrite,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter));

                    Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                        _defaultOverwrite,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter));
                }
                else
                {
                    // Act & Assert FailedResponse
                    Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync,
                        _defaultOverwrite,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                        AzLoggerConstants.WritingNotAllowedMessageKey,
                        AzLoggerConstants.WritingNotAllowedMessage);

                    Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async,
                        _defaultOverwrite,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobName,
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
                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer,
                    azSeverityLevelLogFilter: azSeverityLevelLogFilter));

                Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                    _defaultOverwrite,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobName,
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
                        Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Sync,
                            _defaultOverwrite,
                            _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                            _constantLogBlobContainerName, _constantLogBlobName,
                            entities,
                            delegateContainer: _delegateContainer,
                            azSeverityLevelLogFilter: azSeverityLevelLogFilter),
                            AzLoggerConstants.WritingNotAllowedMessageKey,
                            AzLoggerConstants.WritingNotAllowedMessage);

                        Asserts.FailedResponseWithMessage(await CommonAddToBlob(ConcurrentMethodType.Async,
                            _defaultOverwrite,
                            _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                            _constantLogBlobContainerName, _constantLogBlobName,
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
            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Sync,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
                delegateContainer: _delegateContainer,
                azSeverityLevelLogFilter: azSeverityLevelLogFilter));

            Asserts.SuccessResponse(await CommonAddToBlob(ConcurrentMethodType.Async,
                _defaultOverwrite,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobName,
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
