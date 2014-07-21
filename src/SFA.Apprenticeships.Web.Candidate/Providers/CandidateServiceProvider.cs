namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Users;
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

        public CandidateServiceProvider(
            ICandidateService candidateService,
            IRegistrationService registrationService,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _registrationService = registrationService;
            _mapper = mapper;
        }

        public Candidate Register(RegisterViewModel model)
        {
            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
                _candidateService.Register(candidate, model.Password);

                return candidate;
            }
            catch (Exception)
            {
                return null;
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

        public Candidate Authenticate(LoginViewModel model)
        {
            try
            {
                return _candidateService.Authenticate(model.EmailAddress, model.Password);
            }
            catch (Exception)
            {
                // TODO: LOGGING: do not like consuming exception here (and elsewhere in this class).
                return null;
            }
        }

        public UserStatuses GetUserStatus(string username)
        {
            return _registrationService.GetUserStatus(username);
        }

        public string[] GetRoles(string username)
        {
            var claims = new List<string>();

            switch (GetUserStatus(username))
            {
                case UserStatuses.Active:
                    claims.Add("Activated");
                    break;

                case UserStatuses.PendingActivation:
                    claims.Add("Unactivated");
                    break;
            }

            return claims.ToArray();
        }
    }
}
