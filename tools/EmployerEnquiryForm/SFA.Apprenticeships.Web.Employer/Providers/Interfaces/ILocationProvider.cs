namespace SFA.Apprenticeships.Web.Employer.Providers.Interfaces
{
    using ViewModels;

    public interface ILocationProvider
    {
        LocationsViewModel FindAddress(string postcode); 
    }
}