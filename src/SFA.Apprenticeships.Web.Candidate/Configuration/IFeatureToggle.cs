namespace SFA.Apprenticeships.Web.Candidate.Configuration
{
    public interface IFeatureToggle
    {
        bool IsActive(Feature feature);
    }

    public enum Feature
    {
        SavedSearches
    }
}
