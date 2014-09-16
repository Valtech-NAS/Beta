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
    using NLog;
    using ViewModels.Login;
    using ViewModels.Register;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class CandidateServiceProvider : ICandidateServiceProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationTicketService _authenticationTicketService;

        public CandidateServiceProvider(
            ICandidateService candidateService,
            IUserAccountService userAccountService,
            IAuthenticationTicketService authenticationTicketService,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _userAccountService = userAccountService;
            _authenticationTicketService = authenticationTicketService;
            _mapper = mapper;
        }

        public UserNameAvailability IsUsernameAvailable(string username)
        {
            var userNameAvailability = new UserNameAvailability();

            try
            {
                userNameAvailability.IsUserNameAvailable = _userAccountService.IsUsernameAvailable(username);
            }
            catch ( Exception ex)
            {
                const string errorMessage = "Error checking user name availability";
                var message = string.Format("{0} for {1}", errorMessage, username);
                Logger.ErrorException(message, ex);
                userNameAvailability.HasError = true;
                userNameAvailability.ErrorMessage = errorMessage;
            }

            return userNameAvailability;
        }

        public UserStatusesViewModel GetUserStatus(string username)
        {
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
                Logger.ErrorException(message, e);
                return new UserStatusesViewModel(e.Message);
            }
        }

        public ApplicationStatuses? GetApplicationStatus(Guid candidateId, int vacancyId)
        {
            var application = _candidateService.GetApplications(candidateId)
                .SingleOrDefault(a => a.LegacyVacancyId == vacancyId);

            if (application == null)
            {
                return null;
            }

            return application.Status;
        }

        public bool Register(RegisterViewModel model)
        {
            try
            {
                var candidate = _mapper.Map<RegisterViewModel, Candidate>(model);

                _candidateService.Register(candidate, model.Password);

                SetUserCookies(candidate, UserRoleNames.Unactivated);

                return true;
            }
            catch (CustomException e)
            {
                var message = string.Format("Candidate registration failed for {0}.", model.EmailAddress);

                if (e.Code == ErrorCodes.UserInIncorrectStateError)
                {
                    Logger.InfoException(message, e);
                }
                else {
                    Logger.ErrorException(message, e);
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Candidate registration failed for " + model.EmailAddress, e);
                return false;
            }
        }

        public ActivationViewModel Activate(ActivationViewModel model, Guid candidateId)
        {
            try
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);

                _candidateService.Activate(model.EmailAddress, model.ActivationCode);
                _authenticationTicketService.SetAuthenticationCookie(httpContext.Response.Cookies, candidateId.ToString(),
                    UserRoleNames.Activated);

                return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Activated);
            }
            catch (CustomException e)
            {   
                string message;

                switch (e.Code)
                {
                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserFailed:
                        Logger.ErrorException("Candidate activation failed for " + model.EmailAddress, e);
                        message = ActivationPageMessages.ActivationFailed;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error,
                            message);

                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode:
                        Logger.InfoException("Candidate activation failed for " + model.EmailAddress, e);
                        message = ActivationPageMessages.ActivationCodeIncorrect;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.InvalidCode,
                            message);
                }

            }

            return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error);
        }

        public LoginResultViewModel Login(LoginViewModel model)
        {
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
                Logger.ErrorException("Candidate login failed for " + model.EmailAddress, e);

                return new LoginResultViewModel(LoginPageMessages.LoginFailedErrorText)
                {
                    EmailAddress = model.EmailAddress
                };
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
            try
            {
                Logger.Debug("{0} requested password reset code", model.EmailAddress);

                _userAccountService.SendPasswordResetCode(model.EmailAddress);

                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Send password reset code failed for " + model.EmailAddress, e);

                return false;
            }
        }

        public AccountUnlockViewModel RequestAccountUnlockCode(AccountUnlockViewModel model)
        {
            try
            {
                Logger.Debug("{0} requested account unlock code", model.EmailAddress);
                _userAccountService.ResendAccountUnlockCode(model.EmailAddress);
                return new AccountUnlockViewModel{EmailAddress = model.EmailAddress};
            }
            catch (Exception e)
            {
                var message = string.Format("Send account unlock code failed for " + model.EmailAddress);
                Logger.ErrorException(message, e);
                return new AccountUnlockViewModel(message){EmailAddress = model.EmailAddress};
            }
        }

        public PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel model)
        {
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
                Logger.InfoException("Reset forgotten password failed for " + result.EmailAddress, e);

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

                    default:
                        result.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException("Reset forgotten password failed for " + result.EmailAddress, e);

                result.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
            }

            return result;
        }

        public AccountUnlockViewModel VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
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
                        Logger.WarnException(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.UserInIncorrectState};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeExpired:
                        Logger.WarnException(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.AccountUnlockCodeExpired};
                    case Application.Interfaces.Users.ErrorCodes.AccountUnlockCodeInvalid:
                        Logger.InfoException(e.Message, e);
                        return new AccountUnlockViewModel {Status = AccountUnlockState.AccountUnlockCodeInvalid};
                    default:
                        Logger.ErrorException(e.Message,e);
                        return new AccountUnlockViewModel { Status = AccountUnlockState.Error };
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException("Account unlock failed for " + model.EmailAddress, e);
                return new AccountUnlockViewModel{ Status = AccountUnlockState.Error };
            }
        }

        public bool ResendActivationCode(string username)
        {
            try
            {
                Logger.Debug("{0} requested activation code to be resent", username);

                _userAccountService.ResendActivationCode(username);
                return true;
            }
            catch (CustomException e)
            {
                Logger.InfoException("Reset activation code failed for " + username, e);
                return false;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Reset activation code failed for " + username, e);
                return false;
            }
        }

        public Candidate GetCandidate(string username)
        {
            return _candidateService.GetCandidate(username);
        }

        public Candidate GetCandidate(Guid candidateId)
        {
            return _candidateService.GetCandidate(candidateId);
        }

        #region Helpers

        private void SetUserCookies(Candidate candidate, params string[] roles)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            _authenticationTicketService.SetAuthenticationCookie(httpContext.Response.Cookies,
                candidate.EntityId.ToString(),
                roles);
        }

        #endregion
    }
}
