namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
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

        public bool IsUsernameAvailable(string username)
        {
            return _userAccountService.IsUsernameAvailable(username);
        }

        public UserStatuses GetUserStatus(string username)
        {
            return _userAccountService.GetUserStatus(username);
        }

        public bool Register(RegisterViewModel model)
        {
            try
            {
                Candidate candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
                
                _candidateService.Register(candidate, model.Password);
                SetRegisteredCookies(candidate);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Activate(ActivationViewModel model, string candidateId)
        {
            try
            {
                _candidateService.Activate(model.EmailAddress, model.ActivationCode);
                SetActivatedCookies(candidateId);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Authenticate(LoginViewModel model)
        {
            try
            {
                var candidate = _candidateService.Authenticate(model.EmailAddress, model.Password);

                if (candidate == null)
                {
                    // Incorrect user name or password.
                    return false;
                }

                SetLoggedInCookies(candidate);
                return true;
            }
            catch (Exception ex)
            {
                LogError("Candidate authentication failed for {0}", model.EmailAddress, ex);
                return false;
            }
        }

        public void RequestForgottenPasswordReset(ForgottenPasswordViewModel model)
        {
            try
            {
                _userAccountService.SendPasswordResetCode(model.EmailAddress);
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
                _userAccountService.ResetForgottenPassword(model.EmailAddress, model.PasswordResetCode, model.Password);
                return true;
            }
            catch (InvalidOperationException ioException)
            {
                LogError("Reset forgotten password failed for {0}", model.EmailAddress, ioException);
                throw;
            }
            catch (Exception ex)
            {
                LogError("Reset forgotten password failed for {0}", model.EmailAddress, ex);
                throw ;
            }
        }

        public bool VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            try
            {
                _userAccountService.UnlockAccount(model.EmailAddress, model.AccountUnlockCode);
                return true;
            }
            catch (Exception ex)
            {
                LogError("Account unlock failed for {0}.", model.EmailAddress, ex);
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

        #region Helpers

        // TODO: AG: consolidate cookie setting.
        private void SetLoggedInCookies(Candidate candidate)
        {
            var registrationDetails = candidate.RegistrationDetails;
            var candidateId = candidate.EntityId.ToString();
            var roles = _userAccountService.GetRoleNames(registrationDetails.EmailAddress);

            var httpContext = new HttpContextWrapper(HttpContext.Current);

            _userServiceProvider.SetAuthenticationCookie(
                httpContext, candidateId, roles);

            _userServiceProvider.SetUserContextCookie(
                httpContext, registrationDetails.EmailAddress,
                registrationDetails.FirstName + " " + registrationDetails.LastName);
        }

        private void SetRegisteredCookies(Candidate candidate)
        {
            var registrationDetails = candidate.RegistrationDetails;
            var candidateId = candidate.EntityId.ToString();

            var httpContext = new HttpContextWrapper(HttpContext.Current);

            _userServiceProvider.SetAuthenticationCookie(
                httpContext, candidateId, UserRoleNames.Unactivated);

            _userServiceProvider.SetUserContextCookie(
                httpContext, registrationDetails.EmailAddress,
                registrationDetails.FirstName + " " + registrationDetails.LastName);
        }

        private void SetActivatedCookies(string candidateId)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            _userServiceProvider.SetAuthenticationCookie(
                httpContext, candidateId, UserRoleNames.Activated);
        }

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