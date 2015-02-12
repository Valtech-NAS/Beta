namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.Locations;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Applications;

    public class ApprenticeshipApplicationViewModelBuilder
    {
        private bool _requiresSupportForInterview;
        private string _anythingWeCanDoToSupportYourInterview;
        private ApplicationStatuses _status;
        private string _viewModelMessage;

        public ApprenticeshipApplicationViewModelBuilder RequiresSupportForInterview()
        {
            _requiresSupportForInterview = true;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder DoesNotRequireSupportForInterview()
        {
            _requiresSupportForInterview = false;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder CanBeSupportedAtInterviewBy(string anythingWeCanDoToSupportYourInterview)
        {
            _anythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder HasError(ApplicationStatuses status, string viewModelMessage)
        {
            _status = status;
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipApplicationViewModelBuilder HasError(string viewModelMessage)
        {
            _viewModelMessage = viewModelMessage;
            return this;
        }

        public ApprenticeshipApplicationViewModel Build()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Status = _status,
                ViewModelMessage = _viewModelMessage,
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel(),
                    AboutYou = new AboutYouViewModel
                    {
                        RequiresSupportForInterview = _requiresSupportForInterview,
                        AnythingWeCanDoToSupportYourInterview = _anythingWeCanDoToSupportYourInterview
                    },
                    Address = new AddressViewModel
                    {
                        GeoPoint = new GeoPointViewModel()
                    }
                },
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyAddress = new AddressViewModel
                    {
                        GeoPoint = new GeoPointViewModel()
                    }
                }
            };

            return viewModel;
        }
    }
}