using System;
using System.Globalization;
using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public class VacancySummaryResponse : VacancySummary
    {
        public double Distance { get; set; }
    }
}
