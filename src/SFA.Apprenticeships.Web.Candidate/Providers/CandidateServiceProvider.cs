namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Application.Interfaces.Candidates;
    using System.Web;
    using Application.Interfaces.Users;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using NLog;
    using ViewModels.Login;
    using ViewModels.Register;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IUserAccountService _registrationService;
        private readonly ISessionStateProvider _session;
        private readonly IUserServiceProvider _userServiceProvider;

        public CandidateServiceProvider(
            ISessionStateProvider session,
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IUserServiceProvider userServiceProvider,
            IMapper mapper)
        {
            _session = session;
            _candidateService = candidateService;
            _userAccountService = userAccountService;
            _userServiceProvider = userServiceProvider;
            _mapper = mapper;
        }

        public Candidate Register(RegisterViewModel model)
        {
            try
            {
                Candidate candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
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
            return _userAccountService.IsUsernameAvailable(username);
        }

        public Candidate Authenticate(LoginViewModel model, out UserStatuses userStatus)
        {
            // Set out parameter.
            userStatus = UserStatuses.Unknown;

            try
            {
                var candidate = _candidateService.Authenticate(model.EmailAddress, model.Password);

                if (candidate == null)
                {
                    // Incorrect user name or password.
                
                    return null;
                }

                userStatus = _userAccountService.GetStatus(model.EmailAddress);

                SetAuthenticationCookies(candidate);

                return candidate;
            }
            catch (Exception ex)
            {
                LogError("Candidate authentication failed for {0}", model.EmailAddress, ex);
                return null;
            }
        }

        public void RequestForgottenPasswordReset(ForgottenPasswordViewModel model)
        {
            try
            {
                _registrationService.SendPasswordResetCode(model.EmailAddress);
            }
            catch (Exception ex)
            {
                LogError("Send password reset code failed for {0}", model.EmailAddress, ex);
            }
        }

        public bool VerifyPasswordReset(PasswordResetViewModel model)
        {
            try
            {
                _registrationService.ResetForgottenPassword(model.EmailAddress, model.PasswordResetCode, model.Password);
                return true;
            }
            catch (Exception ex)
            {
                LogError("Reset forgotten password failed for {0}", model.EmailAddress, ex);
                return false;
            }
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

                string stringValue = value.Value.ToString(CultureInfo.InvariantCulture);

                _session.Store(SessionKeys.LastViewedVacancyId, stringValue);
            }
        }

        private void SetAuthenticationCookies(Candidate candidate)
        {
            var registrationDetails = candidate.RegistrationDetails;
            var roles = _userAccountService.GetRoleNames(registrationDetails.EmailAddress);

            var httpContext = new HttpContextWrapper(HttpContext.Current);

            _userServiceProvider.SetAuthenticationCookie(
                httpContext, candidate.EntityId.ToString(), roles);

            _userServiceProvider.SetUserContextCookie(
                httpContext, registrationDetails.EmailAddress, registrationDetails.FullName);
        }

        #region Helpers

        private static void LogError(string formatString, string formatValue, Exception ex)
        {
            var message = string.Format(formatString, formatValue);
            Logger.ErrorException(message, ex);
        }

        private static class SessionKeys
        {
            public const string LastViewedVacancyId = "LastViewedVacancyId";
        }

        #endregion
    }
}