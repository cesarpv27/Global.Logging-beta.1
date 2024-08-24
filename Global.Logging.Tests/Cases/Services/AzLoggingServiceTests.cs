
namespace Global.Logging.Tests
{
    public class AzLoggingServiceTests : AzLoggerBase
    {
        [Fact]
        public void AzTableService_AzBlobService()
        {
            // Act & Assert with AzTableService and AzBlobService
            var argumentNullException = Record.Exception(() =>
            new AzLoggingService<LogEntity>(
                ServiceFactory.CreateAzTableService(_azTableRepositoryMock.MockObject),
                ServiceFactory.CreateAzBlobService(_azBlobRepositoryMock.MockObject)));
            Assert.Null(argumentNullException);
        }

        [Fact]
        public void Null_AzTableService()
        {
            // Act & Assert with null AzTableService and AzBlobService
            var argumentNullException = Assert.Throws<ArgumentNullException>(() =>
            new AzLoggingService<LogEntity>(
#pragma warning disable CS8625
                default,
#pragma warning restore CS8625
                ServiceFactory.CreateAzBlobService(_azBlobRepositoryMock.MockObject)));
            Assert.Equal("azTableService", argumentNullException.ParamName);
        }

        [Fact]
        public void Null_AzBlobService()
        {
            // Act & Assert with AzTableService and null AzBlobService
            var argumentNullException = Assert.Throws<ArgumentNullException>(() =>
            new AzLoggingService<LogEntity>(
                ServiceFactory.CreateAzTableService(_azTableRepositoryMock.MockObject),
#pragma warning disable CS8625
                default));
#pragma warning restore CS8625
            Assert.Equal("azBlobService", argumentNullException.ParamName);
        }
    }
}
