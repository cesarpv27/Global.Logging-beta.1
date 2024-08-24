
namespace Global.Logging.Models
{ 
    /// <summary>
    /// Specifies the type of sequence execution for store operations.
    /// </summary>
    public enum SequenceExecutionType
    {
        /// <summary>
        /// Specifies that the next operation should never be executed regardless of the <see cref="ResponseStatus"/> of the current operation response.
        /// </summary>
        NextNever = 100,

        /// <summary>
        /// Specifies that the next operation should always be executed regardless of the <see cref="ResponseStatus"/> of the current operation response.
        /// </summary>
        NextAlways = 200,

        /// <summary>
        /// Specifies that the next operation should be executed only if the response status of the current operation indicates <see cref="ResponseStatus.Failure"/>.
        /// </summary>
        NextOnFails = 300,

        /// <summary>
        /// Specifies that the next operation should be executed only if the response status of the current operation indicates 
        /// <see cref="ResponseStatus.Success"/> or <see cref="ResponseStatus.Warning"/>.
        /// </summary>
        NextOnComplete = 400,
    }
}
