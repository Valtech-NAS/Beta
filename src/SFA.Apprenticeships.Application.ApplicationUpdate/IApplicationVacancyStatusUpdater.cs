namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Vacancies;

    public interface IApplicationVacancyStatusUpdater
    {
        void Update(Guid candidateId, int vacancyId, VacancyStatuses currentVacancyStatus);
    }
}