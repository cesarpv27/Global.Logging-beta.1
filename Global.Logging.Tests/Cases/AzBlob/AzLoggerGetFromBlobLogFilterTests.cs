﻿
namespace Global.Logging.Tests
{
    public class AzLoggerGetFromBlobLogFilterTests : AzLoggerGetFromBlobBase
    {
        public AzLoggerGetFromBlobLogFilterTests()
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
                    var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobContainerName,
                        _constantLogBlobName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                    LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, syncGetFromResponse.ActualResponse, Extensions.CompareTo);

                    var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobContainerName,
                        _constantLogBlobName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                    LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, asyncGetFromResponse.ActualResponse, Extensions.CompareTo);
                }
                else
                {
                    // Act & Assert FailedResponse
                    var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobContainerName,
                        _constantLogBlobName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                    Asserts.FailedResponseWithMessage(
                        syncGetFromResponse.ActualResponse,
                        AzLoggerConstants.ReadingNotAllowedMessageKey,
                        AzLoggerConstants.ReadingNotAllowedMessage);

                    var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                        _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                        _constantLogBlobContainerName, _constantLogBlobContainerName,
                        _constantLogBlobName, _constantLogBlobName,
                        entities,
                        delegateContainer: _delegateContainer,
                        azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                    Asserts.FailedResponseWithMessage(
                        asyncGetFromResponse.ActualResponse,
                        AzLoggerConstants.ReadingNotAllowedMessageKey,
                        AzLoggerConstants.ReadingNotAllowedMessage);
                }

                Assert.Equal(severityLevel, entities.LogEntity.SeverityLevel);
            }
        }

        #endregion

        [Fact]
        public async Task AllowAnySeverityLevelForReading_SuccessResponse()
        {
            foreach (var allowedSeverityLevel in _severityLevels)
            {
                // Arrange
                var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowSeverityLevelForReadingLogFilter(allowedSeverityLevel);

                var entities = EntityFactory.Create(severityLevel: allowedSeverityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                // Act & Assert SuccessResponse
                Assert.Equal(allowedSeverityLevel, entities.LogEntity.SeverityLevel);

                var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobContainerName,
                    _constantLogBlobName, _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer,
                    azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, syncGetFromResponse.ActualResponse, Extensions.CompareTo);

                var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async, 
                    _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                    _constantLogBlobContainerName, _constantLogBlobContainerName,
                    _constantLogBlobName, _constantLogBlobName,
                    entities,
                    delegateContainer: _delegateContainer,
                    azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                LoggerAsserts.SuccessBlobValueResponse(entities.LogEntity, asyncGetFromResponse.ActualResponse, Extensions.CompareTo);
            }
        }

        [Fact]
        public async Task DenyAnySeverityLevelForReading_FailedResponse()
        {
            foreach (var allowedSeverityLevel in _severityLevels)
            {
                // Arrange
                var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowSeverityLevelForReadingLogFilter(allowedSeverityLevel);

                foreach (var severityLevel in _severityLevels)
                {
                    if (severityLevel != allowedSeverityLevel)
                    {
                        // Arrange
                        var entities = EntityFactory.Create(severityLevel: severityLevel, fillVerboseLabelSet: _delegateContainer.FillVerboseLabelSetDel);

                        // Act & Assert FailedResponse
                        Assert.NotEqual(allowedSeverityLevel, entities.LogEntity.SeverityLevel);

                        var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                            _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                            _constantLogBlobContainerName, _constantLogBlobContainerName,
                            _constantLogBlobName, _constantLogBlobName,
                            entities,
                            delegateContainer: _delegateContainer,
                            azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                        Asserts.FailedResponseWithMessage(
                            syncGetFromResponse.ActualResponse,
                            AzLoggerConstants.ReadingNotAllowedMessageKey,
                            AzLoggerConstants.ReadingNotAllowedMessage);

                        var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                            _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                            _constantLogBlobContainerName, _constantLogBlobContainerName,
                            _constantLogBlobName, _constantLogBlobName,
                            entities,
                            delegateContainer: _delegateContainer,
                            azSeverityLevelLogFilter: azSeverityLevelLogFilter);
                        Asserts.FailedResponseWithMessage(
                            asyncGetFromResponse.ActualResponse,
                            AzLoggerConstants.ReadingNotAllowedMessageKey,
                            AzLoggerConstants.ReadingNotAllowedMessage);
                    }
                }
            }
        }

        [Fact]
        public async Task AllowInfoSeverityLevelForReading_SuccessResponse()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowInfoSeverityLevelForReadingLogFilter();

            // Act & Assert SuccessResponse
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                delegateContainer: _delegateContainer,
                azSeverityLevelLogFilter: azSeverityLevelLogFilter);
            LoggerAsserts.SuccessBlobValueResponse(syncGetFromResponse.ExpectedLogEntity, syncGetFromResponse.ActualResponse, Extensions.CompareTo);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                delegateContainer: _delegateContainer,
                azSeverityLevelLogFilter: azSeverityLevelLogFilter);
            LoggerAsserts.SuccessBlobValueResponse(asyncGetFromResponse.ExpectedLogEntity, asyncGetFromResponse.ActualResponse, Extensions.CompareTo);
        }

        [Fact]
        public async Task AllowLowSeverityLevelForReading()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowLowSeverityLevelForReadingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _lowSeverityLevels);
        }

        [Fact]
        public async Task AllowHighSeverityLevelForReading()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowHighSeverityLevelForReadingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _highSeverityLevels);
        }

        [Fact]
        public async Task AllowErrorSeverityLevelForReading()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowErrorSeverityLevelForReadingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _errorSeverityLevels);
        }

        [Fact]
        public async Task AllowExceptionSeverityLevelForReading()
        {
            // Arrange
            var azSeverityLevelLogFilter = ServiceHelper.GenerateAllowExceptionSeverityLevelForReadingLogFilter();

            // Act & Assert
            await AllowSeverityLevel_SuccessResponse_FailedResponse(azSeverityLevelLogFilter, _exceptionSeverityLevels);
        }
    }
}
