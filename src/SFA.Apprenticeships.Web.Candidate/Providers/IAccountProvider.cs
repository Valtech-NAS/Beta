namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Domain.Entities.Candidates;
    using ViewModels.Account;

    public interface IAccountProvider
    {
        SettingsViewModel GetSettingsViewModel(Guid candidateId);

        bool TrySaveSettings(Guid candidateId, SettingsViewModel model, out Candidate candidate);

        bool DismissTraineeshipPrompts(Guid candidateId);
    }
}
