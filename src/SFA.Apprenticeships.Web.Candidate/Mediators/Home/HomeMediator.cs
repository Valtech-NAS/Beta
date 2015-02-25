namespace SFA.Apprenticeships.Web.Candidate.Mediators.Home
{
    using System;
    using System.Web.Mvc;
    using Common.Constants;
    using Constants.Pages;
    using Providers;
    using Validators;
    using ViewModels.Home;

    public class HomeMediator : MediatorBase, IHomeMediator
    {
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IHomeProvider _homeProvider;
        private readonly ContactMessageServerViewModelValidator _contactMessageServerViewModelValidator;

        public HomeMediator(ICandidateServiceProvider candidateServiceProvider, 
            IHomeProvider homeProvider, 
            ContactMessageServerViewModelValidator contactMessageServerViewModelValidator)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _homeProvider = homeProvider;
            _contactMessageServerViewModelValidator = contactMessageServerViewModelValidator;
        }

        public MediatorResponse<ContactMessageViewModel> SendContactMessage(Guid? candidateId,
            ContactMessageViewModel contactMessageViewModel)
        {
            var validationResult = _contactMessageServerViewModelValidator.Validate(contactMessageViewModel);

            if (!validationResult.IsValid)
            {
                PopulateEnquiries(contactMessageViewModel);
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.ValidationError, contactMessageViewModel, validationResult);
            }

            if (_homeProvider.SendContactMessage(candidateId, contactMessageViewModel))
            {
                var viewModel = GetViewModel(candidateId);
                PopulateEnquiries(viewModel);
                return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                    viewModel, ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success);
            }

            PopulateEnquiries(contactMessageViewModel);
            return GetMediatorResponse(HomeMediatorCodes.SendContactMessage.Error, contactMessageViewModel,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning);
        }

        public MediatorResponse<ContactMessageViewModel> GetContactMessageViewModel(Guid? candidateId)
        {
            var viewModel = GetViewModel(candidateId);
            
            return GetMediatorResponse(HomeMediatorCodes.GetContactMessageViewModel.Successful, viewModel);
        }

        private ContactMessageViewModel GetViewModel(Guid? candidateId)
        {
            var viewModel = new ContactMessageViewModel();
            PopulateEnquiries(viewModel);

            if (candidateId.HasValue)
            {
                try
                {
                    var candidate = _candidateServiceProvider.GetCandidate(candidateId.Value);
                    viewModel.Email = candidate.RegistrationDetails.EmailAddress;
                    viewModel.Name = string.Format("{0} {1}", candidate.RegistrationDetails.FirstName,
                        candidate.RegistrationDetails.LastName);
                }
                catch
                {
                }
            }

            return viewModel;
        }

        private static void PopulateEnquiries(ContactMessageViewModel contactMessageViewModel)
        {
            contactMessageViewModel.Enquiries = GetEnquiries();
            contactMessageViewModel.SelectedEnquiry = "noSelect";
        }

        protected static SelectList GetEnquiries()
        {
            var enquiries = new SelectList(
                new[]
                {
                    new {Id="noSelect", Enquiry = "-- Or choose from one of these questions --" },
                    new {Id="changeEmailAddress", Enquiry = "I need to change my email address" },
                    new {Id="forgottenEmailAddress", Enquiry = "I've forgotten my email address"},
                    new {Id="knowMore", Enquiry = "I would like to know more about eligibility and funding"},
                    new {Id="problemsSubmitting", Enquiry = "I'm having problems submitting my application"},
                    new {Id="noApplication", Enquiry = "I can't see my application"},
                    new {Id="withdrawApplication", Enquiry = "I wish to withdraw an application"},
                    new {Id="deleteAccount", Enquiry = "I wish to delete my account" }
                },
                "Id",
                "Enquiry"
                );

            return enquiries;
        }
    }
}