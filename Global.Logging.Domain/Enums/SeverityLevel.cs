
namespace Global.Logging.Domain
{
    /// <summary>
    /// Enumeration representing the severity levels of log entries.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Informational severity level.
        /// </summary>
        Info = 100,

        /// <summary>
        /// Warning severity level.
        /// </summary>
        Warning = 202,

        /// <summary>
        /// Error severity level.
        /// </summary>
        Error = 400,

        /// <summary>
        /// Fatal error severity level.
        /// </summary>
        FatalError = 403,

        /// <summary>
        /// Exception severity level.
        /// </summary>
        Exception = 500,

        /// <summary>
        /// Fatal exception severity level.
        /// </summary>
        FatalException = 503,
    }
}
