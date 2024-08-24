
namespace Global.Logging.Tests
{
    public class AzBlobServiceTests : AzLoggerBlobBase
    {
        [Fact]
        public void BlobRepository()
        {
            // Arrange with AzBlobRepository
            IAzBlobRepository<LogEntity> azBlobRepository = _azBlobRepositoryMock.MockObject;

            // Act & Assert
            var argumentNullException = Record.Exception(() => new AzBlobService<LogEntity>(azBlobRepository));
            Assert.Null(argumentNullException);
        }

        [Fact]
        public void Null_BlobRepository()
        {
            // Arrange with null AzBlobRepository
            IAzBlobRepository<LogEntity>? azBlobRepository = default;

            // Act & Assert
#pragma warning disable CS8604
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => new AzBlobService<LogEntity>(azBlobRepository));
            Assert.Equal(nameof(azBlobRepository), argumentNullException.ParamName);
#pragma warning restore CS8604
        }
    }
}
