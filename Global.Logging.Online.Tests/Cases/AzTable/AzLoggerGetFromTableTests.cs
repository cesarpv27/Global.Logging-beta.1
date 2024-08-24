
namespace Global.Logging.Online.Tests
{
    public class AzLoggerGetFromTableTests : AzLoggerBase
    {
        protected AzTableValueResponse<ReadOnlyLog> CustomGetFromTable(AzTableResponse response)
        {
            Assert.True(response.TryGetMessage(Constants.TableNameMessageKey, out string? tableName));
            Assert.True(response.TryGetMessage(Constants.PartitionKeyMessageKey, out string? partitionKey));
            Assert.True(response.TryGetMessage(Constants.RowKeyMessageKey, out string? rowKey));

            return _defaultAzLogger.GetFromTable(tableName!, partitionKey!, rowKey!);
        }

        [OnlineFact]
        public async Task AddToTableGetFromTable()
        {
            // Arrange
            int count = Environment.TickCount;
            await _defaultAzLogger.SetFillVerboseLabelSetDelAsync(AzLoggerHelper.GenerateDefaultFillVerboseLabelSet);

            var addToTableResponse0 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Info);

            var addToTableResponse1 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Warning,
                $"{Constants.DefaultMessage}-{count}");

            var addToTableResponse2 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Error,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory);

            var addToTableResponse3 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.FatalError,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            AzTableResponse addToTableResponse4 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Exception,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            var addToTableResponse5 = _defaultAzLogger.AddToTable(
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
            var addToTableResponse6 = _defaultAzLogger.AddToTable(
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
            var getFromTableResponse0 = CustomGetFromTable(addToTableResponse0);
            var getFromTableResponse1 = CustomGetFromTable(addToTableResponse1);
            var getFromTableResponse2 = CustomGetFromTable(addToTableResponse2);
            var getFromTableResponse3 = CustomGetFromTable(addToTableResponse3);
            var getFromTableResponse4 = CustomGetFromTable(addToTableResponse4);
            var getFromTableResponse5 = CustomGetFromTable(addToTableResponse5);
            var getFromTableResponse6 = CustomGetFromTable(addToTableResponse6);

            // Assert
            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse0);
            Assert.Equal(SeverityLevel.Info, getFromTableResponse0.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse0.Value!.Source);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse1);
            Assert.Equal(SeverityLevel.Warning, getFromTableResponse1.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse1.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse1.Value!.Message);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse2);
            Assert.Equal(SeverityLevel.Error, getFromTableResponse2.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse2.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse2.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromTableResponse2.Value!.Category);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse3);
            Assert.Equal(SeverityLevel.FatalError, getFromTableResponse3.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse3.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse3.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromTableResponse3.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromTableResponse3.Value!, _defaultVerbose);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse4);
            Assert.Equal(SeverityLevel.Exception, getFromTableResponse4.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse4.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse4.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromTableResponse4.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromTableResponse4.Value!, _defaultVerbose);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse5);
            Assert.Equal(SeverityLevel.FatalException, getFromTableResponse5.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse5.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse5.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromTableResponse5.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromTableResponse5.Value!, _defaultVerbose);

            LoggerAsserts.SuccessTableValueResponse(getFromTableResponse6);
            Assert.Equal(SeverityLevel.FatalException, getFromTableResponse6.Value!.SeverityLevel);
            Assert.Equal(this.GetType().Name, getFromTableResponse6.Value!.Source);
            Assert.StartsWith(Constants.DefaultMessage, getFromTableResponse6.Value!.Message);
            Assert.Equal(Constants.DefaultCategory, getFromTableResponse6.Value!.Category);
            LoggerAsserts.VerboseLabelSetFields(getFromTableResponse6.Value!, _defaultVerbose);
            LoggerAsserts.ExceptionFields(getFromTableResponse6.Value, testException);
            LoggerAsserts.LabelSetFields(getFromTableResponse6.Value);
            LoggerAsserts.CallStackFields(getFromTableResponse6.Value);
        }
    }
}
