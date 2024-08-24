
namespace Global.Logging.Tests
{
    internal static class ServiceFactory
    {
        public static AzLoggingService<LogEntity> CreateAzLoggingService(
            AzTableRepositoryMock azTableRepositoryMock,
            AzBlobRepositoryMock azBlobRepositoryMock)
        {
            return CreateAzLoggingService(
                CreateAzTableService(azTableRepositoryMock.MockObject),
                CreateAzBlobService(azBlobRepositoryMock.MockObject));
        }

        public static AzLoggingService<LogEntity> CreateAzLoggingService(
            IAzTableService<LogEntity> azTableService, 
            IAzBlobService<LogEntity> azBlobService)
        {
            return new AzLoggingService<LogEntity>(azTableService, azBlobService);
        }

        public static AzTableService<LogEntity> CreateAzTableService(IAzTableRepository<LogEntity> azTableRepository)
        {
            return new AzTableService<LogEntity>(azTableRepository);
        }

        public static AzBlobService<LogEntity> CreateAzBlobService(IAzBlobRepository<LogEntity> azBlobRepository)
        {
            return new AzBlobService<LogEntity>(azBlobRepository);
        }

        public static DelegateContainer<ReadOnlyRawLog<Exception>> CreateDelegateContainer()
        {
            return new DelegateContainer<ReadOnlyRawLog<Exception>>();
        }
    }
}
