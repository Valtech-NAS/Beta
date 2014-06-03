namespace SFA.Apprenticeships.Application.Interfaces.Vacancy
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Vacancy;
    using Domain.Entities.Location;

    public interface IVacancySearchProvider
    {
        IEnumerable<VacancySummary> FindVacancies(Location location, int radius);
    }
}
