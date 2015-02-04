namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class ActivateUserStrategy : IActivateUserStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public ActivateUserStrategy(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public void Activate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState("Activate user", UserStatuses.PendingActivation);

            if (!user.ActivationCode.Equals(activationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException("Invalid activation code \"{0}\" for user \"{1}\"", ErrorCodes.UserActivationCodeError, activationCode, username);
            }

            user.SetStateActive();
            _userWriteRepository.Save(user);
        }
    }
}
