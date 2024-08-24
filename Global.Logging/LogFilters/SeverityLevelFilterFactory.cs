
namespace Global.Logging
{
    /// <summary>
    /// Represents a filters provider for Azure logs based on the <see cref="SeverityLevel"/>.
    /// </summary>
    public static class SeverityLevelFilterFactory
    {
        #region Default SeverityLevel filters

        /// <summary>
        /// Checks if the provided <paramref name="severityLevel"/> if a <see cref="SeverityLevel.Info"/>.
        /// </summary>
        /// <param name="severityLevel">The severity level to check.</param>
        public static bool AllowInfoSeverityLevel(SeverityLevel severityLevel)
        {
            return severityLevel == SeverityLevel.Info;
        }

        /// <summary>
        /// Checks if the provided <paramref name="severityLevel"/> if a any of the low severity levels, which are allowed for logging.
        /// Low severity levels are: 
        /// <see cref="SeverityLevel.Info"/>,
        /// <see cref="SeverityLevel.Warning"/>
        /// </summary>
        /// <param name="severityLevel">The severity level to check.</param>
        public static bool AllowLowSeverityLevel(SeverityLevel severityLevel)
        {
            return severityLevel == SeverityLevel.Info || severityLevel == SeverityLevel.Warning;
        }

        /// <summary>
        /// Checks if the provided <paramref name="severityLevel"/> if a any of the high severity levels, which are allowed for logging.
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>, 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="severityLevel">The severity level to check.</param>
        public static bool AllowHighSeverityLevel(SeverityLevel severityLevel)
        {
            return severityLevel == SeverityLevel.Error
                || severityLevel == SeverityLevel.FatalError
                || severityLevel == SeverityLevel.Exception
                || severityLevel == SeverityLevel.FatalException;
        }

        /// <summary>
        /// Checks if the provided <paramref name="severityLevel"/> if a any of the error severity levels, which are allowed for logging.
        /// Error severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>
        /// </summary>
        /// <param name="severityLevel">The severity level to check.</param>
        public static bool AllowErrorSeverityLevel(SeverityLevel severityLevel)
        {
            return severityLevel == SeverityLevel.Error || severityLevel == SeverityLevel.FatalError;
        }

        /// <summary>
        /// Checks if the provided <paramref name="severityLevel"/> if a any type exception, which are allowed for logging.
        /// Exception severity levels are: 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="severityLevel">The severity level to check.</param>
        public static bool AllowExceptionSeverityLevel(SeverityLevel severityLevel)
        {
            return severityLevel == SeverityLevel.Exception || severityLevel == SeverityLevel.FatalException;
        }

        #endregion
    }
}
