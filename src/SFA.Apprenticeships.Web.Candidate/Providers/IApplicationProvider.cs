namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.Applications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplicationViewModel(int vacancyId, int dummyProfileId);

        void SaveApplication(ApplicationViewModel applicationViewModel);
    }
}
