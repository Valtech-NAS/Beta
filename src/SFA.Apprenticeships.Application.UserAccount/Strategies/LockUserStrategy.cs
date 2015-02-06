namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class LockUserStrategy : ILockUserStrategy
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IUserWriteRepository _userWriteRepository;

        public LockUserStrategy(
            IConfigurationManager configurationManager,
            ICodeGenerator codeGenerator,
            IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            _configurationManager = configurationManager;
            _codeGenerator = codeGenerator;
        }

        public void LockUser(User user)
        {
            // Create and set an unlock code, set code expiry, save user, send email containing unlock code.
            var unlockCodeExpiryDays = _configurationManager.GetAppSetting<int>("UnlockCodeExpiryDays");

            var accountUnlockCode = _codeGenerator.GenerateAlphaNumeric();
            var expiry = DateTime.Now.AddDays(unlockCodeExpiryDays);

            user.SetStateLocked(accountUnlockCode, expiry);
            _userWriteRepository.Save(user);
        }
    }
}
