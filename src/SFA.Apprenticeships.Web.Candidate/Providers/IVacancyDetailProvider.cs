namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.VacancySearch;

    public interface IVacancyDetailProvider
    {
        VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId);
    }
}
