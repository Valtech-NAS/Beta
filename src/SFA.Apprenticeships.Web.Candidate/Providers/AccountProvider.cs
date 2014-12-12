namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Mapping;
    using NLog;
    using ViewModels.Account;
    using ViewModels.Locations;

    public class AccountProvider : IAccountProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public AccountProvider(
            ICandidateService candidateService,
            IMapper mapper)
        {
            _mapper = mapper;
            _candidateService = candidateService;
        }

        public SettingsViewModel GetSettingsViewModel(Guid candidateId)
        {
            try
            {
                Logger.Debug("Calling AccountProvider to get Settings View Model for candidate with Id={0}", candidateId);
                
                var candidate = _candidateService.GetCandidate(candidateId);
                var settings = _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);
                settings.AllowEmailComms = candidate.CommunicationPreferences.AllowEmail;
                return settings;
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Unexpected error while getting settings view model on AccountProvider for candidate with Id={0}.",
                        candidateId);
                Logger.Error(message, e);
                throw;
            }
        }

        public bool SaveSettings(Guid candidateId, SettingsViewModel model)
        {
            try
            {
                Logger.Debug("Calling AccountProvider to save the settings for candidate with Id={0}", candidateId);
                var candidate = _candidateService.GetCandidate(candidateId);

                candidate.CommunicationPreferences.AllowEmail = model.AllowEmailComms;
                PatchRegistrationDetails(candidate.RegistrationDetails, model);
                _candidateService.SaveCandidate(candidate);
                Logger.Debug("Settings saved for candidate with Id={0}", candidateId);

                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Save settings failed for candidate " + candidateId, e);

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
    }
}
