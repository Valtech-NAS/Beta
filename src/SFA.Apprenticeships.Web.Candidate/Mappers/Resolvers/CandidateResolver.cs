namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using ViewModels.Register;
    internal class CandidateResolver : ITypeConverter<RegisterViewModel, Candidate>
    {
        public Candidate Convert(ResolutionContext context)
        {
            var viewModel = (RegisterViewModel)context.SourceValue;

            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = viewModel.Firstname,
                    LastName = viewModel.Lastname,
                    EmailAddress = viewModel.EmailAddress,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = new Address
                    {
                        AddressLine1 = viewModel.Address.AddressLine1,
                        AddressLine2 = viewModel.Address.AddressLine2,
                        AddressLine3 = viewModel.Address.AddressLine3,
                        AddressLine4 = viewModel.Address.AddressLine4,
                        Postcode = viewModel.Address.Postcode,
                        GeoPoint = new GeoPoint
                        {
                            Latitude = viewModel.Address.GeoPoint.Latitude,
                            Longitude = viewModel.Address.GeoPoint.Longitude
                        },
                        Uprn = viewModel.Address.Uprn
                    },
                    DateOfBirth = 
                        new DateTime(viewModel.DateOfBirth.Year.Value, viewModel.DateOfBirth.Month.Value,
                            viewModel.DateOfBirth.Day.Value)
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    AllowEmailMarketing = viewModel.AcceptUpdates
                }
            };

            return candidate;
        }
    }
}