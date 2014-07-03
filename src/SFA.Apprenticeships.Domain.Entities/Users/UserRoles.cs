namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    [Flags]
    public enum UserRoles
    {
        Candidate = 1,
        Employer = 2,
        VacancyManager = 4,
        Provider = 8,
        //todo: add all user roles
    }
}
