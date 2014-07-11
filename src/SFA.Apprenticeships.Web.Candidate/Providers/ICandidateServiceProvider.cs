namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.Register;
    public interface ICandidateServiceProvider
    {
        bool Register(RegisterViewModel model);
        bool Activate(ActivationViewModel model);
        bool IsUsernameAvailable(string username);
    }
}