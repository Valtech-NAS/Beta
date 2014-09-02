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
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using NLog;
    using ViewModels.Login;
    using ViewModels.Register;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;
    using domainExceptions = Domain.Entities.Exceptions;

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
            catch
            {
                userNameAvailability.HasError = true;
                userNameAvailability.ErrorMessage = "Error checking user name availability";
            }

            return userNameAvailability;
        }

        public UserStatuses GetUserStatus(string username)
        {
            return _userAccountService.GetUserStatus(username);
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
                Logger.ErrorException("Candidate activation failed for " + model.EmailAddress, e);
                string message;

                switch (e.Code)
                {
                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserFailed:
                        message = ActivationPageMessages.ActivationFailed;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error,
                            viewModelMessage: message);

                    case Application.Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode:
                        message = ActivationPageMessages.ActivationCodeIncorrect;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.InvalidCode,
                            viewModelMessage: message);
                }

            }

            return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error);
        }

        public LoginResultViewModel Login(LoginViewModel model)
        {
            try
            {
                var userStatus = GetUserStatus(model.EmailAddress);

                if (userStatus == UserStatuses.Locked)
                {
                    return GetLoginResultViewModel(model, userStatus);
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
                        UserStatus = userStatus,
                        IsAuthenticated = true
                    };
                }

                userStatus = GetUserStatus(model.EmailAddress);

                if (userStatus == UserStatuses.Locked)
                {
                    // Authentication failed, user just locked their account.
                    return GetLoginResultViewModel(model, userStatus);
                }

                return GetAuthenticationFailedViewModel(model, userStatus);
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

        public void RequestAccountUnlockCode(AccountUnlockViewModel model)
        {
            try
            {
                Logger.Debug("{0} requested account unlock code", model.EmailAddress);
                _userAccountService.ResendAccountUnlockCode(model.EmailAddress);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Send account unlock code failed for " + model.EmailAddress, e);
                // TODO: fails silently, should return boolean to indicate success
            }
        }

        public PasswordResetViewModel VerifyPasswordReset(PasswordResetViewModel model)
        {
            // TODO: US333: AG: this function changes an object then returns it. Yuk.
            try
            {
                _candidateService.ResetForgottenPassword(model.EmailAddress, model.PasswordResetCode, model.Password);
                model.IsPasswordResetCodeValid = true;
            }
            catch (CustomException e)
            {
                Logger.ErrorException("Reset forgotten password failed for " + model.EmailAddress, e);

                switch (e.Code)
                {
                    case ErrorCodes.UnknownUserError:
                    case ErrorCodes.UserInIncorrectStateError:
                    case ErrorCodes.UserPasswordResetCodeExpiredError:
                    case ErrorCodes.UserPasswordResetCodeIsInvalid:
                        model.IsPasswordResetCodeValid = false;
                        break;

                    case ErrorCodes.UserAccountLockedError:
                        model.UserStatus = UserStatuses.Locked;
                        break;

                    default:
                        model.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.ErrorException("Reset forgotten password failed for " + model.EmailAddress, e);

                model.ViewModelMessage = PasswordResetPageMessages.FailedPasswordReset;
            }

            return model;
        }

        public bool VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            try
            {
                _candidateService.UnlockAccount(model.EmailAddress, model.AccountUnlockCode);
                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Account unlock failed for " + model.EmailAddress, e);
                return false;
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
