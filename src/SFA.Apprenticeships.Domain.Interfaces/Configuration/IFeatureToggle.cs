namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IFeatureToggle
    {
        bool IsActive(Feature feature);
    }

    public enum Feature
    {
        Traineeships
    }
}