using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Collections.Generic;

    public interface IVacancyIndexDataProvider
    {
        int GetVacancyPageCount();

        IEnumerable<VacancySummary> GetVacancySummaries(int page);
    }
}
