
namespace Global.Logging.Domain
{
    /// <summary>
    /// Represents a log with information about an exception.
    /// </summary>
    public interface ILogExceptionInfo
    {
        #region Properties

        /// <summary>
        /// Type of exception associated with the log.
        /// </summary>
        string? ExceptionType { get; }

        /// <summary>
        /// Contains all messages from the exception hierarchy, including the message of 
        /// the top-level exception and all inner exceptions, associated with the log.
        /// </summary>
        string? ExceptionMessages { get; }

        /// <summary>
        /// Contains all stack traces from the exception hierarchy, including the stack trace of 
        /// the top-level exception and all inner exceptions, associated with the log.
        /// </summary>
        string? ExceptionStackTraces { get; }

        /// <summary>
        /// Call stack name of method of the exception associated with the log.
        /// </summary>
        string? ExceptionStackMethod { get; }

        /// <summary>
        /// Call stack line number of the exception associated with the log.
        /// </summary>
        int? ExceptionStackLine { get; }

        /// <summary>
        /// Call stack column number of the exception associated with the log.
        /// </summary>
        int? ExceptionStackColumn { get; }

        #endregion
    }
}
