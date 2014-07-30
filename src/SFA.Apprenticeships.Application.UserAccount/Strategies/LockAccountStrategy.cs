namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class LockAccountStrategy : ILockAccountStrategy
    {
        private readonly ICodeGenerator _codeGenerator;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IConfigurationManager _configurationManager;
        private readonly ISendAccountUnlockCodeStrategy _sendAccountUnlockCodeStrategy;

        public LockAccountStrategy(IUserWriteRepository userWriteRepository,
            IConfigurationManager configurationManager,
            ICodeGenerator codeGenerator,
            ISendAccountUnlockCodeStrategy sendAccountUnlockCodeStrategy)
        {
            _configurationManager = configurationManager;
            _userWriteRepository = userWriteRepository;
            _codeGenerator = codeGenerator;
            _sendAccountUnlockCodeStrategy = sendAccountUnlockCodeStrategy;
        }

        public void LockAccount(User user)
        {
            if (user == null)
            {
                // TODO: AG: do not like to silently consume issues like 'user not found'.
                return;
            }

            // Create and set an unlock code, set code expiry, save user, send email containing unlock code.
            var unlockCodeExpiryDays = _configurationManager.GetAppSetting<int>("UnlockCodeExpiryDays");

            var accountUnlockCode = _codeGenerator.Generate();
            var currentDateTime = DateTime.Now;
            var expiry = currentDateTime.AddDays(unlockCodeExpiryDays);

            user.SetStateLocked(accountUnlockCode, expiry);
            _userWriteRepository.Save(user);

            _sendAccountUnlockCodeStrategy.SendAccountUnlockCode(user.Username);
        }
    }
}