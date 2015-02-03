namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IApplicationVacancyUpdater
    {
        void Update(Guid candidateId, int vacancyId, VacancyDetail vacancyDetail);
    }
}