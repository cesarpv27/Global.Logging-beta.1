
namespace Global.Logging.Online.Tests
{
    public abstract class AzLoggerBase
    {
        protected readonly string _connectionString;
        protected readonly AzLogger _defaultAzLogger;
        protected readonly Dictionary<string, string> _defaultVerbose;
        protected readonly ILogLabelSet _defaultLogLabelSet;
        protected readonly CancellationToken _defaultCancellationToken;

        public AzLoggerBase()
        {
            var connectionStringPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Visual Studio Projects", "Secrets", "sa-dev-shared-access-signature.b.txt");
            _connectionString = File.ReadAllText(connectionStringPath);

            _defaultAzLogger = AzLoggerHelper.CreateAzLogger(_connectionString);

            _defaultVerbose = RawLogHelper.verbose;

            _defaultLogLabelSet = AzLoggerHelper.GenerateDefaultLogLabelSet();
        }
    }
}
