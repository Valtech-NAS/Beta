namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Account;

    public interface IAccountProvider
    {
        SettingsViewModel GetSettingsViewModel(Guid candidateId);
        bool SaveSettings(Guid candidateId, SettingsViewModel model);

        bool DismissTraineeshipPrompts(Guid candidateId);
    }
}
