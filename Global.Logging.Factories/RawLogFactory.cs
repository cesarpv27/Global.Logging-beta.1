
namespace Global.Logging.Factories
{
    /// <summary>
    /// Factory class for creating <see cref="RawLog{TEx}"/> entities.
    /// </summary>
    public static class RawLogFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="RawLog{TEx}"/>.
        /// </summary>
        /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <returns>A new instance of <see cref="RawLog{TEx}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        public static RawLog<TEx> Create<TEx>(
            string source,
            SeverityLevel severityLevel,
            string? message)
            where TEx : Exception
        {
            return new RawLog<TEx>(
                source,
                severityLevel,
                message);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RawLog{TEx}"/>.
        /// </summary>
        /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <returns>A new instance of <see cref="RawLog{TEx}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        public static RawLog<TEx> Create<TEx>(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default)
            where TEx : Exception
        {
            return new RawLog<TEx>(
                source,
                severityLevel,
                message,
                category,
                verbose,
                exception);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RawLog{TEx}"/>.
        /// </summary>
        /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <returns>A new instance of <see cref="RawLog{TEx}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        public static RawLog<TEx> Create<TEx>(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default)
            where TEx : Exception
        {
            return new RawLog<TEx>(
                source,
                severityLevel,
                message,
                category,
                verbose,
                exception,
                callStack,
                callStackMethod,
                callStackLine,
                callStackColumn);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RawLog{TEx}"/>.
        /// </summary>
        /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="logLabelSet">A set of label information used to populate the corresponding label properties (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <returns>A new instance of <see cref="RawLog{TEx}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        public static RawLog<TEx> Create<TEx>(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            ILogLabelSet? logLabelSet,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default)
            where TEx : Exception
        {
            return new RawLog<TEx>(
                source,
                severityLevel,
                message,
                category,
                verbose,
                exception,
                logLabelSet,
                callStack,
                callStackMethod,
                callStackLine,
                callStackColumn);
        }
    }
}
