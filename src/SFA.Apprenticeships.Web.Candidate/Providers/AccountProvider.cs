namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using ViewModels.Account;
    using ViewModels.Locations;

    public class AccountProvider : IAccountProvider
    {
        private readonly ILogService _logger;
        private readonly IFeatureToggle _featureToggle;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public AccountProvider(
            ICandidateService candidateService,
            IMapper mapper, ILogService logger, IFeatureToggle featureToggle)
        {
            _mapper = mapper;
            _logger = logger;
            _featureToggle = featureToggle;
            _candidateService = candidateService;
        }

        public SettingsViewModel GetSettingsViewModel(Guid candidateId)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to get Settings View Model for candidate with Id={0}", candidateId);
                
                var candidate = _candidateService.GetCandidate(candidateId);
                var settings = _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);
                settings.AllowEmailComms = candidate.CommunicationPreferences.AllowEmail;
                settings.AllowSmsComms = candidate.CommunicationPreferences.AllowMobile;
                settings.VerifiedMobile = candidate.CommunicationPreferences.VerifiedMobile;
                settings.SmsEnabled = _featureToggle.IsActive(Feature.Sms);
                return settings;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Unexpected error while getting settings view model on AccountProvider for candidate with Id={0}.",
                        candidateId);
                _logger.Error(message, e);
                throw;
            }
        }

        public bool TrySaveSettings(Guid candidateId, SettingsViewModel model, out Candidate candidate)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to save the settings for candidate with Id={0}", candidateId);
                candidate = _candidateService.GetCandidate(candidateId);

                candidate.CommunicationPreferences.AllowEmail = model.AllowEmailComms;
                candidate.CommunicationPreferences.AllowMobile = model.AllowSmsComms;
                if (candidate.RegistrationDetails.PhoneNumber != model.PhoneNumber)
                {
                    candidate.CommunicationPreferences.VerifiedMobile = false;
                }
                
                PatchRegistrationDetails(candidate.RegistrationDetails, model);
                _candidateService.SaveCandidate(candidate);
                _logger.Debug("Settings saved for candidate with Id={0}", candidateId);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Save settings failed for candidate " + candidateId, e);
                candidate = null;
                return false;
            }
        }

        public bool DismissTraineeshipPrompts(Guid candidateId)
        {
            try
            {
                _logger.Debug("Calling AccountProvider to dismiss traineeship prompts for candidate with Id={0}", candidateId);

                var candidate = _candidateService.GetCandidate(candidateId);

                candidate.CommunicationPreferences.AllowTraineeshipPrompts = false;
                _candidateService.SaveCandidate(candidate);

                _logger.Debug("Settings saved for candidate with Id={0}", candidateId);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("Dismiss traineeship prompts failed for candidate " + candidateId, e);

                return false;
            }
        }

        private void PatchRegistrationDetails(RegistrationDetails registrationDetails, SettingsViewModel model)
        {
            registrationDetails.FirstName = model.Firstname;
            registrationDetails.LastName = model.Lastname;
            registrationDetails.DateOfBirth = new DateTime(
                // ReSharper disable PossibleInvalidOperationException
                model.DateOfBirth.Year.Value, model.DateOfBirth.Month.Value, model.DateOfBirth.Day.Value);
                // ReSharper restore PossibleInvalidOperationException
            registrationDetails.Address = _mapper.Map<AddressViewModel, Address>(model.Address);
            registrationDetails.PhoneNumber = model.PhoneNumber;
        }

        public VerifyMobileViewModel GetVerifyMobileViewModel(Guid candidateId)
        {
            _logger.Debug("Calling CandidateService to fetch candidateId {0} details", candidateId);

            VerifyMobileViewModel model = new VerifyMobileViewModel();
            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                model.PhoneNumber = candidate.RegistrationDetails.PhoneNumber;
                if (!candidate.MobileVerificationRequired())
                {
                    model.Status = VerifyMobileState.MobileVerificationNotRequired;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Mobile code verification failed for candidateId {0} and Lme {1}", candidateId, model.PhoneNumber, e);
                model.ViewModelMessage = e.Message;
                model.Status = VerifyMobileState.Error;
            }
            return model;
        }

        public VerifyMobileViewModel VerifyMobile(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to verify mobile code candidateId {0} and mobile number {1}",
                                                                                    candidateId, model.PhoneNumber);
            try
            {
                _candidateService.VerifyMobileCode(candidateId, model.VerifyMobileCode);
                model.Status =VerifyMobileState.Ok;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        _logger.Info(e.Message, e);
                        model.Status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    case Application.Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed:
                        _logger.Info(e.Message, e);
                        
                        model.Status = VerifyMobileState.VerifyMobileCodeInvalid;
                        break;
                    default:
                        _logger.Error(e.Message, e);
                        model.Status = VerifyMobileState.Error;
                        break;
                }
                model.ViewModelMessage = e.Message;
            }
            catch (Exception e)
            {
                _logger.Error("Mobile code verification failed for candidateId {0} and Lme {1}", candidateId, model.PhoneNumber, e);
                model.Status = VerifyMobileState.Error;
                model.ViewModelMessage = e.Message;
            }
            return model;
        }

        public VerifyMobileViewModel SendMobileVerificationCode(Guid candidateId, VerifyMobileViewModel model)
        {
            _logger.Debug("Calling AccountProvider to send mobile verification code for candidateId {0} to mobile number {1}",
                                                                                    candidateId, model.PhoneNumber);

            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);
                _candidateService.SendMobileVerificationCode(candidate);
                model.Status = VerifyMobileState.Ok;
            }
            catch (CustomException e)
            {
                switch (e.Code)
                {
                    case Domain.Entities.ErrorCodes.EntityStateError:
                        model.Status = VerifyMobileState.MobileVerificationNotRequired;
                        break;
                    default:
                        model.Status = VerifyMobileState.Error;
                        _logger.Error(e.Message, e);
                        break;
                }
                model.ViewModelMessage = e.Message;
            }
            catch (Exception e)
            {
                var message = string.Format("Sending Mobile verification code to mobile number {1} failed for candidateId {0} ", 
                                                                                        candidateId, model.PhoneNumber );
                _logger.Error(message, e);
                
                model.Status = VerifyMobileState.Error;
                model.ViewModelMessage = e.Message;
            }
            return model;
        }
    }
}
        