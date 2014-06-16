namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;

    public interface IVacancyIndexDataProvider
    {
        int GetVacancyPageCount(VacancyLocationType vacancyLocationType);

        IEnumerable<VacancySummary> GetVacancySummaries(VacancyLocationType vacancyLocationType, int page);
    }
}
