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

        public bool IsUsernameAvailable(string username)
        {
            return _userAccountService.IsUsernameAvailable(username);
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
            catch (Exception ex)
            {
                Logger.ErrorException("Candidate registration failed for " + model.EmailAddress, ex);
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
            catch (CustomException ex)
            {
                Logger.ErrorException("Candidate activation failed for " + model.EmailAddress, ex);
                string message = string.Empty;

                switch ( ex.Code )
                {
                    case SFA.Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.ActivateUserFailed:
                        message = ActivationPageMessages.ActivationFailed;
                        return new ActivationViewModel(model.EmailAddress, model.ActivationCode, ActivateUserState.Error,
                            viewModelMessage: message);
                    case SFA.Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode:
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
                    return new LoginResultViewModel
                    {
                        EmailAddress = model.EmailAddress,
                        UserStatus = userStatus
                    };
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
                    return new LoginResultViewModel
                    {
                        EmailAddress = model.EmailAddress,
                        UserStatus = userStatus
                    };
                }

                // Authentication failed, user's account is not locked yet.
                return new LoginResultViewModel(LoginPageMessages.InvalidUsernameOrPasswordErrorText)
                {
                    EmailAddress = model.EmailAddress,
                    UserStatus = userStatus
                };
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
            catch (Exception ex)
            {
                Logger.ErrorException("Send account unlock code failed for " + model.EmailAddress, ex);
                // TODO: fails silently, should return boolean to indicate success
            }
        }

        public void VerifyPasswordReset(PasswordResetViewModel model)
        {
            try
            {
                _candidateService.ResetForgottenPassword(model.EmailAddress, model.PasswordResetCode, model.Password);
            }
            catch (CustomException ex)
            {
                //todo: catch more specific custom errors first
                Logger.ErrorException("Reset forgotten password failed for " + model.EmailAddress, ex);
                throw;
            }
        }

        public bool VerifyAccountUnlockCode(AccountUnlockViewModel model)
        {
            try
            {
                _candidateService.UnlockAccount(model.EmailAddress, model.AccountUnlockCode);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Account unlock failed for " + model.EmailAddress, ex);
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
            catch (CustomException ex)
            {
                Logger.ErrorException("Reset activation code failed for " + username, ex);
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
