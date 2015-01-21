namespace SFA.Apprenticeships.Web.Candidate.Mediators.Account
{
    using System;
    using ViewModels.Account;
    using ViewModels.MyApplications;

    public interface IAccountMediator
    {
        MediatorResponse<MyApplicationsViewModel> Index(Guid candidateId, string deletedVacancyId, string deletedVacancyTitle);

        MediatorResponse Archive(Guid candidateId, int vacancyId);

        MediatorResponse Delete(Guid candidateId, int vacancyId);

        MediatorResponse DismissTraineeshipPrompts(Guid candidateId);

        MediatorResponse<SettingsViewModel> Settings(Guid candidateId);

        MediatorResponse<SettingsViewModel> Settings(Guid candidateId, SettingsViewModel settingsViewModel);

        MediatorResponse Track(Guid candidateId, int vacancyId);
    }
}