namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipApplicationDetail : ApplicationDetail
    {
        public ApprenticeshipApplicationDetail()
        {
            Vacancy = new ApprenticeshipSummary();
        }

        public ApprenticeshipSummary Vacancy { get; set; }
    }
}