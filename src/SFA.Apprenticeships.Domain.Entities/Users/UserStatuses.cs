namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public enum UserStatuses
    {
        Unknown = 0,
        PendingActivation = 10,     // once registered and awaiting activation
        Active = 20,                // once activated
        Inactive = 30,              // when superseded by a new account (user changed their email address)
        Locked = 90                 // if locked out for security reasons
    }
}
