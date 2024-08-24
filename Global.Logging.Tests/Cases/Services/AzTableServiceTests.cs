
namespace Global.Logging.Tests
{
    public class AzTableServiceTests : AzLoggerTableBase
    {
        [Fact]
        public void TableRepository()
        {
            // Arrange with AzTableRepository
            IAzTableRepository<LogEntity> azTableRepository = _azTableRepositoryMock.MockObject;

            // Act & Assert
            var argumentNullException = Record.Exception(() => new AzTableService<LogEntity>(azTableRepository));
            Assert.Null(argumentNullException);
        }

        [Fact]
        public void Null_TableRepository()
        {
            // Arrange with null AzTableRepository
            IAzTableRepository<LogEntity>? azTableRepository = default;

            // Act & Assert
#pragma warning disable CS8604
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => new AzTableService<LogEntity>(azTableRepository));
            Assert.Equal(nameof(azTableRepository), argumentNullException.ParamName);
#pragma warning restore CS8604
        }
    }
}
