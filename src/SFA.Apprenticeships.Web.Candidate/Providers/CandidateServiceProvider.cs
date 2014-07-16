namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Login;
    using ViewModels.Register;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Mapping;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private readonly IRegistrationService _registrationService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public CandidateServiceProvider(ICandidateService candidateService, IRegistrationService registrationService, IMapper mapper)
        {
            _candidateService = candidateService;
            _registrationService = registrationService;
            _mapper = mapper;
        }

        public bool Register(RegisterViewModel model)
        {
            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
                _candidateService.Register(candidate, model.Password);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Activate(ActivationViewModel model)
        {
            try
            {
                _candidateService.Activate(model.EmailAddress, model.ActivationCode);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        public bool IsUsernameAvailable(string username)
        {
            return _registrationService.IsUsernameAvailable(username);
        }

        public bool Authenticate(LoginViewModel model)
        {
            try
            {
                var candidate = _candidateService.Authenticate(model.EmailAddress, model.Password);

                return candidate != null;
            }
            catch (Exception)
            {
                // TODO: LOGGING: AG: do not like consuming exception here (and elsewhere in this class).
                return false;
            }
        }
    }
}
