namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Users;
    using ViewModels.Login;
    using ViewModels.Register;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Mapping;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private static class SessionKeys
        {
            public const string LastViewedVacancyId = "LastViewedVacancyId";
        }

        private readonly ISessionStateProvider _session;
        private readonly IUserAccountService _registrationService;
        private readonly ICandidateService _candidateService;
        private readonly IUserServiceProvider _userServiceProvider;
        private readonly IMapper _mapper;

        public CandidateServiceProvider(
            ISessionStateProvider session,
            ICandidateService candidateService,
            IUserAccountService registrationService,
            IUserServiceProvider userServiceProvider,
            IMapper mapper)
        {
            _session = session;
            _candidateService = candidateService;
            _registrationService = registrationService;
            _userServiceProvider = userServiceProvider;
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
            catch (Exception)
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
                    claims.Add(UserRoleNames.Activated);
                    break;

                case UserStatuses.PendingActivation:
                    claims.Add(UserRoleNames.Unactivated);
                    break;
            }

            return claims.ToArray();
        }

        public int? LastViewedVacancyId
        {
            get
            {
                var stringValue = _session.Get<string>(SessionKeys.LastViewedVacancyId);

                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return null;
                }

                int intValue;

                if (!Int32.TryParse(stringValue, out intValue))
                {
                    return null;
                }

                return intValue;
            }

            set
            {
                if (value == null)
                {
                    _session.Delete(SessionKeys.LastViewedVacancyId);
                    return;
                }

                var stringValue = value.Value.ToString(CultureInfo.InvariantCulture);

                _session.Store(
                    SessionKeys.LastViewedVacancyId, stringValue);
            }
        }
    }
}
