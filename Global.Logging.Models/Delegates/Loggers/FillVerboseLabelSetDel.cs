
namespace Global.Logging.Models
{
    /// <summary>
    /// Delegate for filling a <paramref name="verboseLabelSet"/> with data for categorization or identification purposes, 
    /// using data from the specified <paramref name="verbose"/> collection.
    /// The <paramref name="verboseLabelSet"/> will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.
    /// </summary>
    /// <param name="verboseLabelSet">The verbose label set instance to be filled.</param>
    /// <param name="verbose">The read-only dictionary which can be used to collect the data to populate the instance of <see cref="IVerboseLabelSet"/>.</param>
    /// <remarks>
    /// The delegate is typically used to populate an instance of <see cref="IVerboseLabelSet"/> with relevant information 
    /// for categorization or identification purposes, using data from the specified <paramref name="verbose"/> collection..
    /// </remarks>
    public delegate void FillVerboseLabelSetDel(VerboseLabelSet verboseLabelSet, ReadOnlyDictionary<string, string> verbose);
}
