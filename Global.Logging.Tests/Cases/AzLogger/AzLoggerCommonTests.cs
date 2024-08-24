
namespace Global.Logging.Tests
{
    public class AzLoggerCommonTests : AzLoggerBase
    {
        [Fact]
        public void AzLoggerParams()
        {
            // Act & Assert
            var exception = Record.Exception(() => new AzLogger(_azLoggingService, _defaultLogFactory));
            Assert.Null(exception);
            exception = Record.Exception(() => new AzLogger(_azLoggingService, _defaultLogFactory, _defaultAzSeverityLevelLogFilter));
            Assert.Null(exception);
        }

        [Fact]
        public void Null_AzLoggingServiceNull_LogFactory()
        {
            // Act & Assert
#pragma warning disable CS8625
            var exception = Assert.Throws<ArgumentNullException>(() => new AzLogger(default, _defaultLogFactory));
            Assert.Equal("azLoggingService", exception.ParamName);
            exception = Assert.Throws<ArgumentNullException>(() => new AzLogger(_azLoggingService, default));
            Assert.Equal("logFactory", exception.ParamName);
#pragma warning disable CS8625
        }
    }
}
