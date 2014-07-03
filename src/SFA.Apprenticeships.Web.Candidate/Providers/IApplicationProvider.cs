namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.Applications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplicationViewModel(int vacancyId, int mockProfileId);

        ApplicationViewModel MergeApplicationViewModel(int vacancyId, int mockProfileId, ApplicationViewModel userApplicationViewModel);

        void SaveApplication(ApplicationViewModel applicationViewModel);
    }
}
