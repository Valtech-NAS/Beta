namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public enum VacancyStatuses
    {
        Unknown = 0,

        // Current vacancy which can be applied for.
        Live = 1,

        // Vacancy which can no longer be applied for: Deleted, Closing Date Passed, Withdrawn,
        // Completed, Posted In Error etc.
        Unavailable = 2
    }
}
