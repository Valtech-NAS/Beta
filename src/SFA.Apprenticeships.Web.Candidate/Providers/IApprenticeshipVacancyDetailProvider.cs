namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipVacancyDetailProvider
    {
        VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);
    }
}
