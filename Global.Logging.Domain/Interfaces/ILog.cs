
namespace Global.Logging.Domain
{
    /// <summary>
    /// An interface defining the required properties for a log model.
    /// </summary>
    public interface ILog
    {
        #region Properties

        /// <summary>
        /// Source of the log.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Severity level of the log.
        /// </summary>
        SeverityLevel SeverityLevel { get; }

        /// <summary>
        /// Message associated with the log.
        /// </summary>
        string? Message { get; }

        /// <summary>
        /// Category associated with the log.
        /// </summary>
        string? Category { get; }

        /// <summary>
        /// Call stack associated with the source of the log.
        /// </summary>
        string? CallStack { get; }

        /// <summary>
        /// Call stack method associated with the source of the log.
        /// </summary>
        string? CallStackMethod { get; }

        /// <summary>
        /// Call stack line number associated with the source of the log.
        /// </summary>
        int? CallStackLine { get; }

        /// <summary>
        /// Call stack column number associated with the source of the log.
        /// </summary>
        int? CallStackColumn { get; }

        #endregion
    }
}
