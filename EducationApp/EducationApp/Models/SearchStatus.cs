namespace EducationApp.Models
{
    public enum SearchStatus : byte
    {
        Inactive,
        Searching,
        NoResults,
        ResultsAvailable,
        Faulted
    }
}