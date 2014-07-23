namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Messaging;
    using Interfaces.Users;

    public class LockAccountStrategy : ILockAccountStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;

        public LockAccountStrategy(IUserWriteRepository userWriteRepository, ICodeGenerator codeGenerator,
            ICommunicationService communicationService)
        {
            _userWriteRepository = userWriteRepository;
            _codeGenerator = codeGenerator;
            _communicationService = communicationService;
        }

        public void LockAccount(User user)
        {
            //todo: create and set an unlock code, set code expiry, save user, send email containing unlock code
            var accountUnlockCode = _codeGenerator.Generate();

            user.AccountUnlockCode = accountUnlockCode;
            user.Status = UserStatuses.Locked;

            //_userWriteRepository.Save(user);

            //_communicationService.SendMessageToCandidate(); SendAccountUnlockCode

            throw new NotImplementedException();
        }
    }
}
