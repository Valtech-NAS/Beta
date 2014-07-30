namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Domain.Entities.Users;

    public interface ILockUserStrategy
    {
        void LockUser(User user);
    }
}
