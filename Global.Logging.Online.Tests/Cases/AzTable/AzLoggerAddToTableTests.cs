
namespace Global.Logging.Online.Tests
{
    public class AzLoggerAddToTableTests : AzLoggerBase
    {
        [OnlineFact]
        public void AddToTable()
        {
            // Arrange
            int count = Environment.TickCount;

            // Act
            var response0 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Info);

            var response1 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Warning,
                $"{Constants.DefaultMessage}-{count}");

            var response2 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Error,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory);

            var response3 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.FatalError,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            var response4 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.Exception,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            var response5 = _defaultAzLogger.AddToTable(
                this.GetType().Name,
                SeverityLevel.FatalException,
                $"{Constants.DefaultMessage}-{++count}",
                Constants.DefaultCategory,
                verbose: _defaultVerbose);

            // Assert            
            Asserts.SuccessResponse(response0);
            Asserts.SuccessResponse(response1);
            Asserts.SuccessResponse(response2);
            Asserts.SuccessResponse(response3);
            Asserts.SuccessResponse(response4);
            Asserts.SuccessResponse(response5);
        }
    }
}
