namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using System.Web;
    using Application.Interfaces.Users;
    using Common.Constants;
    using Common.Services;
    using Constants.Pages;
    using Constants.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using Microsoft.WindowsAzure;
    using NLog;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;
    using ViewModels.Login;
    using ViewModels.Register;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private const string WebRolePerformanceCounterCategory = "SFA.Apprenticeships.Web.Candidate";
        private const string CandidateRegistrationCounter = "CandidateRegistration";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly IPerformanceCounterService _performanceCounterService;
        private readonly HttpContextBase _httpContext;

        public CandidateServiceProvider(
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IAuthenticationTicketService authenticationTicketService,
            IMapper mapper, 
            HttpContextBase httpContext, 
            IPerformanceCounterService performanceCounterService)
        {
            _candidateService = candidateService;
            _userAccountService = userAccountService;
            _authenticationTicketService = authenticationTicketService;
            _mapper = mapper;
            _httpContext = httpContext;
            _performanceCounterService = performanceCounterService;
        }

        public UserNameAvailability IsUsernameAvailable(string username)
        {
            Logger.Debug("Calling CandidateServiceProvider to if the username {0} is available.", username);

            var userNameAvailability = new UserNameAvailability();

            try
            {
                userNameAvailability.IsUserNameAvailable = _userAccountService.IsUsernameAvailable(username);
            }
            catch ( Exception ex)
            {
                const string errorMessage = "Error checking user name availability";
                var message = string.Format("{0} for {1}", errorMessage, username);
                Logger.Error(message, ex);

                userNameAvailability.HasError = true;
                userNameAvailability.ErrorMessage = errorMessage;
            }

            return userNameAvailability;
        }

        public UserStatusesViewModel GetUserStatus(string username)
        {
            Logger.Debug("Calling CandidateServiceProvider to get the user status for the user {0}.", username);

            try
            {
                return new UserStatusesViewModel
                {
                    UserStatus = _userAccountService.GetUserStatus(username)
                };
            }
            catch (Exception e)
            {
                const string errorMessage = "Error getting user status";
                var message = string.Format("{0} for {1}", errorMessage, username);
                Logger.Error(message, e);

                return new UserStatusesViewModel(e.Message);
            }
        }

        public ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling CandidateServiceProvider to get the application status for CandidateID={0}, VacancyId={1}.",
                candidateId, vacancyId);

            try
            {
                var application = _candidateService.GetApplications(candidateId)
                    .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);

                if (application == null)
                {
                    return null;
                }

                return application.Status;
            }
            catch (Exception e)
            {
                var message = string.Format("Get Application status failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);
                throw;
            }
        }

        public bool Register(RegisterViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to register a new candidate");

            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);

                _candidateService.Register(candidate, model.Password);

                SetUserCookies(candidate, UserRoleNames.Unactivated);

                IncrementCandidateRegistrationCounter();

                return true;
            }
            catch (CustomException e)
            {
                var message = string.Format("Candidate registration failed for {0}.", model.EmailAddress);

                if (e.Code == ErrorCodes.UserInIncorrectStateError)
                {
                    Logger.Info(message, e);
                }
                else {
                    Logger.Error(message, e);
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("Candidate registration failed for " + model.EmailAddress, e);
                return false;
            }
        }

        public ActivationViewModel Activate(ActivationViewModel model, Guid candidateId)
        {
            Logger.Debug(
                "Calling CandidateServiceProvider to activate user with Id={0}",
                candidateId);

            try
            {
                _candidateService.Activate(model.EmailAddress, model.ActivationCode);
                _authenticationTicketService.SetAuthenticationCookie(_httpContext.Response.Cookies,
                    candidateId.ToString(),
                    UserRoleNames.Activated);

                return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Activated);
            }
            catch (CustomException e)
            {
                string message;

                switch (e.Code)
                {
                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserFailed:
                        Logger.Error("Candidate activation failed for " + model.EmailAddress, e);
                        message = ActivationPageMessages.ActivationFailed;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error,
                            message);

                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode:
                        Logger.Info("Candidate activation failed for " + model.EmailAddress, e);
                        message = ActivationPageMessages.ActivationCodeIncorrect;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode,
                            ActivateUserState.InvalidCode,
                            message);
                    default:
                        Logger.Error("Candidate activation failed for " + model.EmailAddress, e);
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Candidate activation failed for " + model.EmailAddress, e);
                throw;
            }

            return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error);
        }

        public LoginResultViewModel Login(LoginViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to log the user {0}",
                model.EmailAddress);

            try
            {
                var userStatusViewModel = GetUserStatus(model.EmailAddress);

                if (userStatusViewModel.UserStatus == UserStatuses.Locked)
                {
                    return GetLoginResultViewModel(model, userStatusViewModel.UserStatus);
                }

                var candidate = _candidateService.Authenticate(model.EmailAddress, model.Password);
               
                if (candidate != null)
                {
                    // User is authentic.
                    SetUserCookies(candidate, _userAccountService.GetRoleNames(model.EmailAddress));

                    return new LoginResultViewModel
                    {
                        EmailAddress = candidate.RegistrationDetails.EmailAddress,
                        FullName = candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
                        UserStatus = userStatusViewModel.UserStatus,
                        IsAuthenticated = true
                    };
                }

                userStatusViewModel = GetUserStatus(model.EmailAddress);

                if (userStatusViewModel.UserStatus == UserStatuses.Locked)
                {
                    // Authentication failed, user just locked their account.
                    return GetLoginResultViewModel(model, userStatusViewModel.UserStatus);
                }

                return GetAuthenticationFailedViewModel(model, userStatusViewModel.UserStatus);
            }
            catch (Exception e)
            {
                Logger.Error("Candidate login failed for " + model.EmailAddress, e);

                return new LoginResultViewModel(LoginPageMessages.LoginFailedErrorText)
                {
                    EmailAddress = model.EmailAddress
                };
            }
        }

        private void IncrementCandidateRegistrationCounter()
        {
            bool performanceCountersEnabled;

            if (bool.TryParse(CloudConfigurationManager.GetSetting("PerformanceCountersEnabled"), out performanceCountersEnabled)
                && performanceCountersEnabled)
            {
                _performanceCounterService.IncrementCounter(WebRolePerformanceCounterCategory, CandidateRegistrationCounter);
            }
        }

        private static LoginResultViewModel GetLoginResultViewModel(LoginViewModel model, UserStatuses userStatus)
        {
            return new LoginResultViewModel
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        private static LoginResultViewModel GetAuthenticationFailedViewModel(LoginViewModel model, UserStatuses userStatus)
        {
            // Authentication failed, user's account is not locked yet.
            return new LoginResultViewModel(LoginPageMessages.InvalidUsernameOrPasswordErrorText)
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        public bool RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to request a password reset code for user {0}", model.EmailAddress);

            try
            {
                _userAccountService.SendPasswordResetCode(model.EmailAddress);

                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Send password reset code failed for " + model.EmailAddress, e);

                return false;
            }
        }

        public AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to request an account unlock code for user {0}",
                model.EmailAddress);

            try
            {   
                _userAccountService.ResendAccountUnlockCode(model.EmailAddress);
                return new AccountUnlockViewModel{EmailAddress = model.EmailAddress};
            }
            catch (Exception e)
            {
                var message = string.Format("Send account unlock code failed for " + model.EmailAddress);
                Logger.Error(message, e);
                return new AccountUnlockViewModel(message){EmailAddress = model.EmailAddress};
            }
        }

        public PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to verify password reset for user {0}",
                model.EmailAddress);

            var result = new PasswordResetViewModel
            {
                EmailAddress = model.EmailAddress,
                PasswordResetCode = model.PasswordResetCode,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                UserStatus = model.UserStatus,
                IsPasswordResetCodeValid = false
            };

            try
            {
                _candidateService.ResetForgottenPassword(result.EmailAddress, result.PasswordResetCode, result.Password);
                result.IsPasswordResetCodeValid = true;
            }
            catch (CustomException e)
            {
                Logger.Info("Reset forgotten password failed for " + result.EmailAddress, e);

                switch (e.Code)
                {
                    case ErrorCodes.UnknownUserError:
                    case ErrorCodes.UserInIncorrectStateError:
                    case ErrorCodes.UserPasswordResetCodeExpiredError:
                    case ErrorCodes.UserPasswordResetCodeIsInvalid:
                        result.IsPasswordResetCodeValid = false;
                        break;

                    case ErrorCodes.UserAccountLockedError:
                        result.UserStatus = UserStatuses.Locked;
                        break;

                    case ErrorCodes.CandidateCreationError:
                        result.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        Logger.Error("Reset forgotten password failed for " + result.EmailAddress, e);
                        break;
                    default:
                        result.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Reset forgotten password failed for " + result.EmailAddress, e);

                result.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
            }

            return result;
        }

        public AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            Logger.Debug("Calling CandidateServiceProvider to verify account unlock code for user {0}",
                model.EmailAddress);

            try
            {
                _candidateService.UnlockAccount(model.EmailAddress, model.AccountUnlockCode);
                return new AccountUnlockViewModel {Status = AccountUnlockState.Ok};
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case ErrorCodes.UserInIncorrectStateError:
                        Logger.Warn(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.UserInIncorrectState};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeExpired:
                        Logger.Warn(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.AccountUnlockCodeExpired};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeInvalid:
                        Logger.Info(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.AccountUnlockCodeInvalid};
                    default:
                        Logger.Error(e.Message,e);
                        return new AccountUnlockViewModel { Status = AccountUnlockState.Error };
                }
            }
            catch (Exception e)
            {
                Logger.Error("Account unlock failed for " + model.EmailAddress, e);
                return new AccountUnlockViewModel{ Status = AccountUnlockState.Error };
            }
        }

        public bool ResendActivationCode(string username)
        {
            Logger.Debug("Calling CandidateServiceProvider to request activation code for user {0}.", username);

            try
            {
                _userAccountService.ResendActivationCode(username);
                return true;
            }
            catch (CustomException e)
            {
                Logger.Info("Reset activation code failed for " + username, e);
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("Reset activation code failed for " + username, e);
                return false;
            }
        }

        public Candidate GetCandidate(string username)
        {
            Logger.Debug("Calling CandidateServiceProvider to get Candidate for user {0}.", username);

            try
            {
                return _candidateService.GetCandidate(username);
            }
            catch (Exception e)
            {
                var message = string.Format("GetCandidate for user {0} failed.", username);
                Logger.Error(message, e);
                throw;
            }
        }

        public Candidate GetCandidate(Guid candidateId)
        {
            try
            {
                return _candidateService.GetCandidate(candidateId);
            }
            catch (Exception e)
            {
                var message = string.Format("GetCandidate for user with Id={0} failed.", candidateId);
                Logger.Error(message, e);
                throw;
            }
        }

        #region Helpers

        private void SetUserCookies(Candidate candidate, params string[] roles)
        {
            _authenticationTicketService.SetAuthenticationCookie(_httpContext.Response.Cookies,
                candidate.EntityId.ToString(),
                roles);
        }

        #endregion
    }
}
