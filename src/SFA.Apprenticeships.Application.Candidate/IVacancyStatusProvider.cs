namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IVacancyStatusProvider
    {
        VacancyStatuses GetVacancyStatus(int vacancyId);
    }
}
