using CuttingEdge.Conditions;

namespace SFA.Apprenticeships.Application.Vacancy.Service
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Vacancy;
    using Domain.Entities.Vacancy;

    public class VacancySummary : IVacancyProvider
    {
        private readonly IVacancyProvider _service;

        public VacancySummary(IVacancyProvider service)
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
