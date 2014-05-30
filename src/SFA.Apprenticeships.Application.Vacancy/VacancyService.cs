namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public class VacancyService : IVacancyService
    {
        private readonly IVacancyProvider _service;

        public VacancyService(IVacancyProvider service)
        {
            Condition.Requires(service).IsNotNull();

            _service = service;
        }

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            return _service.GetVacancyPageCount(vacancyLocationType);
        }

        public IEnumerable<Domain.Entities.Vacancy.VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page = 1)
        {
            return _service.GetVacancySummary(vacancyLocationType, page);
        }
    }
}
