namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipVacancyDetailProvider
    {
        VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);
    }
}
