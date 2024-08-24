
namespace Global.Logging.Online.Tests
{
    public class AzLoggerGetFromBlobTests : AzLoggerBase
    {
        protected AzBlobValueResponse<ReadOnlyLog> CustomGetFromBlob(AzBlobResponse response)
        {
            Assert.True(response.TryGetMessage(Constants.BlobContainerNameMessageKey, out string? blobContainerName));
            Assert.True(response.TryGetMessage(Constants.BlobNameMessageKey, out string? blobName));

            return _defaultAzLogger.GetFromBlob(blobContainerName!, blobName!);
        }

        [OnlineFact]
        public async Task AddToBlobGetFromBlob()
        {
            // Arrange
            int count = Environment.TickCount;
            await _defaultAzLogger.SetFillVerboseLabelSetDelAsync(AzLoggerHelper.GenerateDefaultFillVerboseLabelSet);

            var addToBlobResponse0 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.Info);

            var addToBlobResponse1 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.Warning,
                $"{Constants.DefaultMessage}-{count}");

            var addToBlobResponse2 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.Error,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory);

            var addToBlobResponse3 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.FatalError,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            AzBlobResponse addToBlobResponse4 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.Exception,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            var addToBlobResponse5 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.FatalException,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            Exception? testException;
            try
            {
                throw new ArgumentException(Constants.TestExceptionMessage);
            }
            catch (Exception e)
            {
                testException = e;
            }
            var addToBlobResponse6 = _defaultAzLogger.AddToBlob(
                this.GetType().Name,
                SeverityLevel.FatalException,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose,
                testException!,
                _defaultLogLabelSet,
                Constants.DefaultCallStackMessage,
                Constants.DefaultCallStackMethodMessage,
                Constants.DefaultCallStackLineMessage,
                Constants.DefaultCallStackColumnMessage);

            // Act
            var getFromBlobResponse0 = CustomGetFromBlob(addToBlobResponse0);
            var getFromBlobResponse1 = CustomGetFromBlob(addToBlobResponse1);
            var getFromBlobResponse2 = CustomGetFromBlob(addToBlobResponse2);
            var getFromBlobResponse3 = CustomGetFromBlob(addToBlobResponse3);
            var getFromBlobResponse4 = CustomGetFromBlob(addToBlobResponse4);
            var getFromBlobResponse5 = CustomGetFromBlob(addToBlobResponse5);
            var getFromBlobResponse6 = CustomGetFromBlob(addToBlobResponse6);

            // Assert
            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse0);
            Assert.Equal(SeverityLevel.Info, getFromBlobResponse0.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse0.Value!.Source);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse1);
            Assert.Equal(SeverityLevel.Warning, getFromBlobResponse1.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse1.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse1.Value!.Message);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse2);
            Assert.Equal(SeverityLevel.Error, getFromBlobResponse2.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse2.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse2.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromBlobResponse2.Value!.Category);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse3);
            Assert.Equal(SeverityLevel.FatalError, getFromBlobResponse3.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse3.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse3.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromBlobResponse3.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromBlobResponse3.Value!, _defaultVerbose);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse4);
            Assert.Equal(SeverityLevel.Exception, getFromBlobResponse4.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse4.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse4.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromBlobResponse4.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromBlobResponse4.Value!, _defaultVerbose);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse5);
            Assert.Equal(SeverityLevel.FatalException, getFromBlobResponse5.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse5.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse5.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromBlobResponse5.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromBlobResponse5.Value!, _defaultVerbose);

            LoggerAsserts.SuccessBlobValueResponse(getFromBlobResponse6);
            Assert.Equal(SeverityLevel.FatalException, getFromBlobResponse6.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromBlobResponse6.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromBlobResponse6.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromBlobResponse6.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromBlobResponse6.Value!, _defaultVerbose);
            LoggerAsserts.ExceptionFields(getFromBlobResponse6.Value, testException);
            LoggerAsserts.LabelSetFields(getFromBlobResponse6.Value);
            LoggerAsserts.CallStackFields(getFromBlobResponse6.Value);
        }
    }
}
