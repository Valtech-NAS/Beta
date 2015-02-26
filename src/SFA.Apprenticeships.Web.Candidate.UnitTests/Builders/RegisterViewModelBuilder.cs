namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels;
    using Candidate.ViewModels.Locations;
    using Candidate.ViewModels.Register;

    public class RegisterViewModelBuilder
    {
        private bool _doesNotAcceptUpdates;

        public RegisterViewModelBuilder DoesNotAcceptUpdates()
        {
            _doesNotAcceptUpdates = true;
            return this;
        }

        public RegisterViewModel Build()
        {
            var model = new RegisterViewModel
            {
                Address = new AddressViewModel
                {
                    GeoPoint = new GeoPointViewModel()
                },
                DateOfBirth = new DateViewModel
                {
                    Day = 01,
                    Month = 01,
                    Year = 1985
                }
            };

            if (_doesNotAcceptUpdates)
            {
                model.AcceptUpdates = false;
            }

            return model;
        }
    }
}