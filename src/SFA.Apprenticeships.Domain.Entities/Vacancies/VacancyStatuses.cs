namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;

    public enum VacancyStatuses
    {
        Unknown,
        Live,           // current vacancy which can be applied for
        Withdrawn,      // withdrawn from vacancy manager / provider / employer
        Expired         // past the closing date for applications
    }
}
