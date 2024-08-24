
namespace Global.Logging.Tests
{
    internal class MockFactory
    {
        public static AzTableRepositoryMock CreateAzTableRepositoryMock()
        {
            return new AzTableRepositoryMock();
        }

        public static AzBlobRepositoryMock CreateAzBlobRepositoryMock()
        {
            return new AzBlobRepositoryMock();
        }

        public static AzTableServiceMock CreateAzTableServiceMock(IAzTableRepository<LogEntity> azTableRepository)
        {
            return new AzTableServiceMock(azTableRepository);
        }

        public static AzBlobServiceMock CreateAzBlobServiceMock(IAzBlobRepository<LogEntity> azTableRepository)
        {
            return new AzBlobServiceMock(azTableRepository);
        }
    }
}
