
namespace Global.Logging.Tests
{
    internal static class EntityHelper
    {
        public static string GenerateDefaultPartitionKey(IReadOnlyRawLog readOnlyRawLog)
        {
            return DateTime.Today.Day.ToString();
        }

        public static string GenerateDefaultRowKey(IReadOnlyRawLog readOnlyRawLog)
        {
            return string.Format(Constants.RowKeyFormat, readOnlyRawLog.SeverityLevel, readOnlyRawLog.Source, Guid.NewGuid());
        }

        public static string GenerateConstantPartitionKey(IReadOnlyRawLog? readOnlyRawLog = default)
        {
            return "ConstantPartitionKey";
        }

        public static string GenerateConstantRowKey(IReadOnlyRawLog? readOnlyRawLog = default)
        {
            return "ConstantRowKey";
        }
    }
}
