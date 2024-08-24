
namespace Global.Logging.Domain
{
    /// <summary>
    /// An interface defining the required properties for a log entity model.
    /// </summary>
    public interface ILogEntity : ITableEntity, ILog, ILogExceptionInfo, ILogLabelSet, IVerboseLabelSet
    {
        #region Properties

        /// <summary>
        /// Constains additional information associated with the log.
        /// </summary>
        string? Verbose { get; set; }

        #endregion
    }
}
