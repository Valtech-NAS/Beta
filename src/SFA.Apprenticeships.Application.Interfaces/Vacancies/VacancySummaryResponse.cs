namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies;

    public class VacancySummaryResponse : VacancySummary
    {
        public double Distance { get; set; }

        public double Score { get; set; }
    }
}
