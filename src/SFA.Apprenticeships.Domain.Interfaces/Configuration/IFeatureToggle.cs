namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IFeatureToggle
    {
        bool IsActive(string featureName);
    }
}