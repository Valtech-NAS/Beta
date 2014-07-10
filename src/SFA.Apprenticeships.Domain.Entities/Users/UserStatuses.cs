namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public enum UserStatuses
    {
        Unknown = 0,
        PendingActivation = 10,     // once registered and awaiting activation
        Active = 20,                // once activated
        Inactive = 30,              // when superseded by a new account
        Blocked = 90                // if locked out for security reasons
    }
}
