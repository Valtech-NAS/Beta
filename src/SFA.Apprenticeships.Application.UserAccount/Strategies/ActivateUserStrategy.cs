namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

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

            user.AssertState("Only users in a Pending Activation state can activate this account", UserStatuses.PendingActivation);

            if (!user.ActivationCode.Equals(activationCode, StringComparison.InvariantCultureIgnoreCase))
                throw new CustomException("Invalid activation code", ErrorCodes.UserActivationCodeError); 

            user.SetStateActive();
            _userWriteRepository.Save(user);
        }
    }
}
