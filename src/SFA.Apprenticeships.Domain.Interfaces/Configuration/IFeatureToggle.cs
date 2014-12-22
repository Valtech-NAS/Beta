namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    //todo: move the toggle into the web layer as it's only used there
    public interface IFeatureToggle
    {
        bool IsActive(Feature feature);
    }

    public enum Feature
    {
        Traineeships
    }
}
