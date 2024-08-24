
namespace Global.Logging.Tests
{
    internal static class Extensions
    {
        #region LogEntity - ReadOnlyLog

        public static bool CompareTo(this LogEntity? @this, LogEntity? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            return
                // ITableEntity
                string.Equals(@this!.PartitionKey, other!.PartitionKey, StringComparison.InvariantCulture)
                && string.Equals(@this.RowKey, other.RowKey, StringComparison.InvariantCulture)
                // ILogEntity
                && string.Equals(@this.Verbose, other.Verbose, StringComparison.InvariantCulture)
                // ILog
                && @this.CompareLogTo(other)
                // ILogExceptionInfo
                && @this.CompareLogExceptionInfoTo(other)
                // ILogLabelSet
                && @this.CompareLogLabelSetTo(other)
                // IVerboseLabelSet
                && @this.CompareVerboseLabelSetTo(other);
        }

        public static bool CompareTo(this LogEntity? logEntity, ReadOnlyLog? readOnlyLog)
        {
            if (logEntity is null && readOnlyLog is null)
                return true;
            if (logEntity is null && readOnlyLog is not null
                || logEntity is not null && readOnlyLog is null)
                return false;

            return
                // ITableEntity
                string.Equals(logEntity!.PartitionKey, readOnlyLog!.PartitionKey, StringComparison.InvariantCulture)
                && string.Equals(logEntity.RowKey, readOnlyLog.RowKey, StringComparison.InvariantCulture)
                // ILogEntity
                && string.Equals(logEntity.Verbose, JsonSerializer.Serialize(readOnlyLog.Verbose), StringComparison.InvariantCulture)
                // ILog
                && logEntity.CompareLogTo(readOnlyLog)
                // ILogExceptionInfo
                && logEntity.CompareLogExceptionInfoTo(readOnlyLog)
                // ILogLabelSet
                && logEntity.CompareLogLabelSetTo(readOnlyLog)
                // IVerboseLabelSet
                && logEntity.CompareVerboseLabelSetTo(readOnlyLog);
        }

        public static bool CompareLogTo(this ILog? @this, ILog? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            return
                // ILog Mandatory
                string.Equals(@this!.Source, other!.Source, StringComparison.InvariantCulture)
                && @this.SeverityLevel == other.SeverityLevel
                // ILog Optional
                && string.Equals(@this.Message, other.Message, StringComparison.InvariantCulture)
                && string.Equals(@this.Category, other.Category, StringComparison.InvariantCulture)
                && string.Equals(@this.CallStack, other.CallStack, StringComparison.InvariantCulture)
                && string.Equals(@this.CallStackMethod, other.CallStackMethod, StringComparison.InvariantCulture)
                && @this.CallStackLine == other.CallStackLine
                && @this.CallStackColumn == other.CallStackColumn;
        }

        public static bool CompareLogExceptionInfoTo(this ILogExceptionInfo? @this, ILogExceptionInfo? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            return
                string.Equals(@this!.ExceptionType, other!.ExceptionType, StringComparison.InvariantCulture)
                && string.Equals(@this.ExceptionMessages, other.ExceptionMessages, StringComparison.InvariantCulture)
                && string.Equals(@this.ExceptionStackTraces, other.ExceptionStackTraces, StringComparison.InvariantCulture)
                && string.Equals(@this.ExceptionStackMethod, other.ExceptionStackMethod, StringComparison.InvariantCulture)
                && @this.ExceptionStackLine == other.ExceptionStackLine
                && @this.ExceptionStackColumn == other.ExceptionStackColumn;
        }

        public static bool CompareLogLabelSetTo(this ILogLabelSet? @this, ILogLabelSet? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            return 
                string.Equals(@this!.Label0, other!.Label0, StringComparison.InvariantCulture)
                && string.Equals(@this.Label1, other.Label1, StringComparison.InvariantCulture)
                && string.Equals(@this.Label2, other.Label2, StringComparison.InvariantCulture)
                && string.Equals(@this.Label3, other.Label3, StringComparison.InvariantCulture)
                && string.Equals(@this.Label4, other.Label4, StringComparison.InvariantCulture)
                && string.Equals(@this.Label5, other.Label5, StringComparison.InvariantCulture)
                && string.Equals(@this.Label6, other.Label6, StringComparison.InvariantCulture)
                && string.Equals(@this.Label7, other.Label7, StringComparison.InvariantCulture)
                && string.Equals(@this.Label8, other.Label8, StringComparison.InvariantCulture)
                && string.Equals(@this.Label9, other.Label9, StringComparison.InvariantCulture);
        }

        public static bool CompareVerboseLabelSetTo(this IVerboseLabelSet? @this, IVerboseLabelSet? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            return
                string.Equals(@this!.VerboseLabel0, other!.VerboseLabel0, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel1, other.VerboseLabel1, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel2, other.VerboseLabel2, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel3, other.VerboseLabel3, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel4, other.VerboseLabel4, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel5, other.VerboseLabel5, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel6, other.VerboseLabel6, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel7, other.VerboseLabel7, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel8, other.VerboseLabel8, StringComparison.InvariantCulture)
                && string.Equals(@this.VerboseLabel9, other.VerboseLabel9, StringComparison.InvariantCulture);
        }

        #endregion

        #region IDictionary<string, string>

        public static bool CompareTo(this IDictionary<string, string>? @this, IDictionary<string, string>? other)
        {
            if (@this is null && other is null)
                return true;
            if (@this is null && other is not null
                || @this is not null && other is null)
                return false;

            var jsonThis = JsonSerializer.Serialize(@this);
            var jsonOther = JsonSerializer.Serialize(other);

            return string.Equals(jsonThis, jsonOther);
        }

        #endregion
    }
}
