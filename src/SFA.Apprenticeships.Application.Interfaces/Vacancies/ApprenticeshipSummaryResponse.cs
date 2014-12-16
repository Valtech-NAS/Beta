namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipSummaryResponse : ApprenticeshipSummary
    {
        public double Distance { get; set; }

        public double Score { get; set; }
    }
}
