namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;

    public class RegisterUserStrategy : IRegisterUserStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly int _activationCodeExpiryDays;

        public RegisterUserStrategy(IUserWriteRepository userWriteRepository, IConfigurationManager configurationManager)
        {
            _userWriteRepository = userWriteRepository;
            _activationCodeExpiryDays = configurationManager.GetAppSetting<int>("ActivationCodeExpiryDays");
        }

        public void Register(string username, Guid userId, string activationCode, UserRoles roles)
        {
            // todo: check username not "already in use and in a not pending state". if so, throw ex
            //var user = _userReadRepository ...

            var newUser = new User
            {
                EntityId = userId,
                Username = username,
                Roles = roles
            };

            newUser.SetStatePendingActivation(activationCode, DateTime.Now.AddDays(_activationCodeExpiryDays));

            _userWriteRepository.Save(newUser);
        }
    }
}
