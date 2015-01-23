namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public enum VacancyStatuses
    {
        Unknown = 0,

        // Current vacancy which can be applied for.
        Live = 1,

        // Vacancy which can no longer be applied for.
        Unavailable = 2,

        // Vacancy which has expired but can still be viewed.
        Expired = 3
    }
}
