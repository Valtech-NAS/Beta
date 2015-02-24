namespace SFA.Apprenticeships.Web.Employer.Mediators.Interfaces
{
    using ViewModels;

    public interface ILocationMediator
    {
        MediatorResponse<LocationsViewModel> FindAddress(string postcode);
    }
}