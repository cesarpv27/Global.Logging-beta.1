
namespace Global.Logging.Domain.Abstractions
{
    /// <summary>
    /// Defines the common functionalities for a log.
    /// </summary>
    public abstract class PartialLog : ILog, ILogLabelSet
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialLog"/> class. 
        /// Reserved for derived classes.
        /// </summary>
#pragma warning disable CS8618
        protected PartialLog() { }
#pragma warning restore

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialLog"/> class. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        public PartialLog(
            string source,
            SeverityLevel severityLevel)
        {
            if (source == null)
                throw new ArgumentNullException(source);

            Source = source;
            SeverityLevel = severityLevel;
        }

        #endregion

        #region ILog properties

        /// <inheritdoc/>
        public virtual string Source { get; set; }

        /// <inheritdoc/>
        public virtual SeverityLevel SeverityLevel { get; set; }

        /// <inheritdoc/>
        public virtual string? Message { get; set; }

        /// <inheritdoc/>
        public virtual string? Category { get; set; }

        /// <inheritdoc/>
        public virtual string? CallStack { get; set; }

        /// <inheritdoc/>
        public virtual string? CallStackMethod { get; set; }

        /// <inheritdoc/>
        public virtual int? CallStackLine { get; set; }

        /// <inheritdoc/>
        public virtual int? CallStackColumn { get; set; }

        #endregion

        #region ILogLabelSet properties

        /// <inheritdoc/>
        public virtual string? Label0 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label1 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label2 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label3 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label4 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label5 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label6 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label7 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label8 { get; set; }

        /// <inheritdoc/>
        public virtual string? Label9 { get; set; }

        #endregion
    }
}
