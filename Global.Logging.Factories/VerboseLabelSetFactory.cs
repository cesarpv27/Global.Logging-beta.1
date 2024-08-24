
namespace Global.Logging.Factories
{
    /// <summary>
    /// Factory class for creating <see cref="VerboseLabelSetFactory"/> entities.
    /// </summary>
    public static class VerboseLabelSetFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="VerboseLabelSet"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="VerboseLabelSet"/>.</returns>
        public static VerboseLabelSet Create()
        {
            return new VerboseLabelSet();
        }
    }
}
