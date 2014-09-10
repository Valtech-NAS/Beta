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
            var candidate = _candidateService.GetCandidate(candidateId);

            return _mapper.Map<RegistrationDetails, SettingsViewModel>(candidate.RegistrationDetails);
        }

        public bool SaveSettings(Guid candidateId, SettingsViewModel model)
        {
            try
            {
                var candidate = _candidateService.GetCandidate(candidateId);

                PatchRegistrationDetails(candidate.RegistrationDetails, model);
                _candidateService.SaveCandidate(candidate);

                return true;
            }
            catch (Exception e)
            {
                Logger.ErrorException("Save settings failed for candidate " + candidateId, e);

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
