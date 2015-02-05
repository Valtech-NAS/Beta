namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Linq;
    using System.Web;
    using Application.Interfaces.Logging;
    using Common.Providers;
    using Constants;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Constants.Pages;
    using ViewModels;
    using ViewModels.Login;
    using ViewModels.Register;
    using Common.Constants;
    using Common.Services;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;
    using UserErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private readonly ILogService _logger;
        private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly ICandidateService _candidateService;
        private readonly IConfigurationManager _configurationManager;
        private readonly HttpContextBase _httpContext;
        private readonly IMapper _mapper;
        private readonly IUserAccountService _userAccountService;
        private readonly IUserDataProvider _userDataProvider;

        public CandidateServiceProvider(
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IUserDataProvider userDataProvider,
            IAuthenticationTicketService authenticationTicketService,
            IMapper mapper,
            HttpContextBase httpContext,
            IConfigurationManager configurationManager, ILogService logger)
        {
            _candidateService = candidateService;
            _userAccountService = userAccountService;
            _userDataProvider = userDataProvider;
            _authenticationTicketService = authenticationTicketService;
            _mapper = mapper;
            _httpContext = httpContext;
            _configurationManager = configurationManager;
            _logger = logger;
        }

        public UserNameAvailability IsUsernameAvailable(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to if the username {0} is available.", username);

            var userNameAvailability = new UserNameAvailability();

            try
            {
                userNameAvailability.IsUserNameAvailable = _userAccountService.IsUsernameAvailable(username);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error checking user name availability for {0}";
                var message = string.Format(errorMessage, username);
                _logger.Error(message, ex);

                userNameAvailability.HasError = true;
                userNameAvailability.ErrorMessage = errorMessage;
            }

            return userNameAvailability;
        }

        public UserStatusesViewModel GetUserStatus(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to get the user status for the user {0}.", username);

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
                _logger.Error(message, e);

                return new UserStatusesViewModel(e.Message);
            }
        }

        public ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling CandidateServiceProvider to get the application status for CandidateID={0}, VacancyId={1}.",
                candidateId, vacancyId);

            try
            {
                var application = _candidateService.GetApprenticeshipApplications(candidateId)
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

                _logger.Error(message, e);
                throw;
            }
        }

        public bool Register(RegisterViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to register a new candidate");

            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = _configurationManager.GetAppSetting<string>(Settings.TermsAndConditionsVersion);

                _candidateService.Register(candidate, model.Password);

                SetUserCookies(candidate, UserRoleNames.Unactivated);

                return true;
            }
            catch (CustomException e)
            {
                var message = string.Format("Candidate registration failed for {0}.", model.EmailAddress);

                if (e.Code == Application.Interfaces.Users.ErrorCodes.UserInIncorrectStateError)
                {
                    _logger.Info(message, e);
                }
                else
                {
                    _logger.Error(message, e);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Candidate registration failed for " + model.EmailAddress, e);
                return false;
            }
        }

        public ActivationViewModel Activate(ActivationViewModel model, Guid candidateId)
        {
            _logger.Info(
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
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Error("Candidate was in an invalid state for activation:" + model.EmailAddress, e);
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error, ActivationPageMessages.ActivationFailed);

                    default:
                        _logger.Info("Candidate activation failed for " + model.EmailAddress, e);
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.InvalidCode, ActivationPageMessages.ActivationCodeIncorrect);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Candidate activation failed for " + model.EmailAddress, e);
                return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error, ActivationPageMessages.ActivationFailed);
            }
        }

        public LoginResultViewModel Login(LoginViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to log the user {0}",
                model.EmailAddress);

            try
            {
                var userStatusViewModel = GetUserStatus(model.EmailAddress);

                if (userStatusViewModel.UserStatus.HasValue)
                {
                    if (userStatusViewModel.UserStatus == UserStatuses.Locked)
                    {
                        return GetLoginResultViewModel(model, userStatusViewModel.UserStatus.Value);
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
                            UserStatus = userStatusViewModel.UserStatus.Value,
                            IsAuthenticated = true,
                            AcceptedTermsAndConditionsVersion = candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion
                        };
                    }

                    userStatusViewModel = GetUserStatus(model.EmailAddress);

                    if (userStatusViewModel.UserStatus == UserStatuses.Locked)
                    {
                        // Authentication failed, user just locked their account.
                        return GetLoginResultViewModel(model, userStatusViewModel.UserStatus.Value);
                    }
                }

                return GetAuthenticationFailedViewModel(model, userStatusViewModel.UserStatus);
            }
            catch (Exception e)
            {
                _logger.Error("Candidate login failed for " + model.EmailAddress, e);

                return new LoginResultViewModel(LoginPageMessages.LoginFailedErrorText)
                {
                    EmailAddress = model.EmailAddress
                };
            }
        }

        public bool RequestForgottenPasswordResetCode(ForgottenPasswordViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to request a password reset code for user {0}",
                model.EmailAddress);

            try
            {
                _userAccountService.SendPasswordResetCode(model.EmailAddress);

                return true;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Application.Interfaces.Users.ErrorCodes.UserInIncorrectStateError:
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        _logger.Info(e.Message, e);
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        break;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Send password reset code failed for " + model.EmailAddress, e);

                return false;
            }
        }

        public AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to request an account unlock code for user {0}",
                model.EmailAddress);

            try
            {
                _userAccountService.ResendAccountUnlockCode(model.EmailAddress);
                return new AccountUnlockViewModel {EmailAddress = model.EmailAddress};
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Application.Interfaces.Users.ErrorCodes.UserInIncorrectStateError:
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        _logger.Info(e.Message, e);
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        break;
                }
                return new AccountUnlockViewModel(e.Message) {EmailAddress = model.EmailAddress};
            }
            catch (Exception e)
            {
                var message = string.Format("Send account unlock code failed for " + model.EmailAddress);
                _logger.Error(message, e);
                return new AccountUnlockViewModel(message) {EmailAddress = model.EmailAddress};
            }
        }

        public PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel passwordResetViewModel)
        {
            _logger.Debug("Calling CandidateServiceProvider to verify password reset for user {0}", passwordResetViewModel.EmailAddress);
            passwordResetViewModel.IsPasswordResetCodeValid = false;

            try
            {
                _candidateService.ResetForgottenPassword(passwordResetViewModel.EmailAddress, passwordResetViewModel.PasswordResetCode, passwordResetViewModel.Password);

                passwordResetViewModel.IsPasswordResetCodeValid = true;
                passwordResetViewModel.UserStatus = UserStatuses.Active;
            }
            catch (CustomException e)
            {
                _logger.Info("Reset forgotten password failed for " + passwordResetViewModel.EmailAddress, e);

                switch (e.Code)
                {
                    case UserErrorCodes.UnknownUserError:
                    case Application.Interfaces.Users.ErrorCodes.UserInIncorrectStateError:
                    case Application.Interfaces.Users.ErrorCodes.UserPasswordResetCodeExpiredError:
                    case Application.Interfaces.Users.ErrorCodes.UserPasswordResetCodeIsInvalid:
                        passwordResetViewModel.IsPasswordResetCodeValid = false;
                        break;

                    case Application.Interfaces.Users.ErrorCodes.UserAccountLockedError:
                        passwordResetViewModel.UserStatus = UserStatuses.Locked;
                        break;

                    case CandidateErrorCodes.CandidateCreationError:
                        passwordResetViewModel.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        _logger.Error("Reset forgotten password failed for " + passwordResetViewModel.EmailAddress, e);
                        break;
                    default:
                        passwordResetViewModel.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Reset forgotten password failed for " + passwordResetViewModel.EmailAddress, e);

                passwordResetViewModel.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
            }

            return passwordResetViewModel;
        }

        public AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            _logger.Debug("Calling CandidateServiceProvider to verify account unlock code for user {0}",
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
                    case Application.Interfaces.Users.ErrorCodes.UserInIncorrectStateError:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.UserInIncorrectState};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeExpired:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.AccountUnlockCodeExpired};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeInvalid:
                    case Application.Interfaces.Users.ErrorCodes.UnknownUserError:
                        _logger.Info(e.Message, e);
                        return new AccountUnlockViewModel
                        {
                            Status = AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid
                        };
                    default:
                        _logger.Error(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.Error};
                }
            }
            catch (Exception e)
            {
                _logger.Error("Account unlock failed for " + model.EmailAddress, e);
                return new AccountUnlockViewModel {Status = AccountUnlockState.Error};
            }
        }

        public bool ResendActivationCode(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to request activation code for user {0}.", username);

            try
            {
                _userAccountService.ResendActivationCode(username);
                return true;
            }
            catch (CustomException e)
            {
                _logger.Info("Reset activation code failed for " + username, e);
                return false;
            }
            catch (Exception e)
            {
                _logger.Error("Reset activation code failed for " + username, e);
                return false;
            }
        }

        public Candidate GetCandidate(string username)
        {
            _logger.Debug("Calling CandidateServiceProvider to get Candidate for user {0}.", username);

            try
            {
                return _candidateService.GetCandidate(username);
            }
            catch (Exception e)
            {
                var message = string.Format("GetCandidate for user {0} failed.", username);
                _logger.Error(message, e);
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
                _logger.Error(message, e);
                throw;
            }
        }

        public bool AcceptTermsAndConditions(Guid candidateId, string currentVersion)
        {
            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = currentVersion;
                _candidateService.SaveCandidate(candidate);
                _userDataProvider.SetUserContext(candidate.RegistrationDetails.EmailAddress, candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName, currentVersion);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating terms and conditions version", ex);
                return false;
            }
        }

        #region Helpers

        private void SetUserCookies(Candidate candidate, params string[] roles)
        {
            _authenticationTicketService.SetAuthenticationCookie(_httpContext.Response.Cookies,
                candidate.EntityId.ToString(),
                roles);
        }


        private static LoginResultViewModel GetLoginResultViewModel(LoginViewModel model, UserStatuses userStatus)
        {
            return new LoginResultViewModel
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        private static LoginResultViewModel GetAuthenticationFailedViewModel(LoginViewModel model, UserStatuses? userStatus)
        {
            return new LoginResultViewModel(LoginPageMessages.InvalidUsernameOrPasswordErrorText)
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };
        }

        #endregion
    }
}