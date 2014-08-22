namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using ViewModels;
    using ViewModels.Account;
    using ViewModels.Locations;

    public static class SettingsViewModelResolvers
    {
        public class RegistrationDetailsToSettingsViewModelResolver : ITypeConverter<RegistrationDetails, SettingsViewModel>
        {
            public SettingsViewModel Convert(ResolutionContext context)
            {
                var registrationDetails = (RegistrationDetails)context.SourceValue;

                var model = new SettingsViewModel
                {
                    Firstname = registrationDetails.FirstName,
                    Lastname = registrationDetails.LastName,
                    Address = context.Engine.Map<Address, AddressViewModel>(registrationDetails.Address),
                    DateOfBirth = new DateViewModel
                    {
                        Day = registrationDetails.DateOfBirth.Day,
                        Month = registrationDetails.DateOfBirth.Month,
                        Year = registrationDetails.DateOfBirth.Year
                    },
                    PhoneNumber = registrationDetails.PhoneNumber
                };

                return model;
            }
        }
    }
}
