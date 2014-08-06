namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IVacancyStatusProvider
    {
        VacancyStatuses GetVacancyStatus(int vacancyId);
    }
}
