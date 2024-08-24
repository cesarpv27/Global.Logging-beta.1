
namespace Global.Logging.Tests
{
    public abstract class AzLoggerBase
    {
        protected readonly AzTableRepositoryMock _azTableRepositoryMock;
        protected readonly AzBlobRepositoryMock _azBlobRepositoryMock;

        protected readonly AzLoggingService<LogEntity> _azLoggingService;

        protected readonly LogFactory<Exception> _defaultLogFactory;
        protected readonly IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> _defaultAzSeverityLevelLogFilter;
        protected readonly CancellationToken _defaultCancellationToken;

        protected readonly DelegateContainer<ReadOnlyRawLog<Exception>> _delegateContainer;
        protected readonly List<SeverityLevel> _severityLevels;
        protected readonly List<SeverityLevel> _lowSeverityLevels;
        protected readonly List<SeverityLevel> _highSeverityLevels;
        protected readonly List<SeverityLevel> _errorSeverityLevels;
        protected readonly List<SeverityLevel> _exceptionSeverityLevels;

        protected bool? _retryOnFailures;
        protected int? _maxRetryAttempts;

        public AzLoggerBase()
        {
            _azTableRepositoryMock = MockFactory.CreateAzTableRepositoryMock();
            _azBlobRepositoryMock = MockFactory.CreateAzBlobRepositoryMock();

            _azLoggingService = ServiceFactory.CreateAzLoggingService(_azTableRepositoryMock, _azBlobRepositoryMock);

            _defaultLogFactory = ServiceHelper.GenerateLogFactory();
            _defaultAzSeverityLevelLogFilter = ServiceHelper.GenerateDefaultAzSeverityLevelLogFilter();
            _defaultCancellationToken = default;


            _delegateContainer = ServiceFactory.CreateDelegateContainer();
            _delegateContainer.GeneratePartitionKeyDel = EntityHelper.GenerateConstantPartitionKey;
            _delegateContainer.GenerateRowKeyDel = EntityHelper.GenerateConstantRowKey;

            _severityLevels = Enum.GetValues<SeverityLevel>().ToList();
            _lowSeverityLevels = _severityLevels.Where(sLevel => sLevel <= SeverityLevel.Warning).ToList();
            _highSeverityLevels = _severityLevels.Where(sLevel => sLevel > SeverityLevel.Warning).ToList();
            _errorSeverityLevels = _severityLevels.Where(sLevel => sLevel == SeverityLevel.Error || sLevel == SeverityLevel.FatalError).ToList();
            _exceptionSeverityLevels = _severityLevels.Where(sLevel => sLevel == SeverityLevel.Exception || sLevel == SeverityLevel.FatalException).ToList();

            _retryOnFailures = false;
        }
    }
}
