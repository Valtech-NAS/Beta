namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public enum VacancyStatuses
    {
        Unknown,

        // Current vacancy which can be applied for.
        Live,

        // Vacancy which can no longer be applied for: Deleted, Closing Date Passed, Withdrawn,
        // Completed, Posted In Error etc.
        Unavailable     
    }
}
