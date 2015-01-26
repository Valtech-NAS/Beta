namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using Application.Candidate;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LegacyCandidateProviderIntegrationTests
    {
        private ILegacyCandidateProvider _legacyCandidateProvider;

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });
#pragma warning restore 0618

#pragma warning disable 618
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
#pragma warning restore 618
        }

        [Test, Category("Integration")]
        public void ShouldCreateCandidateUsingLegacyCandidateProvider()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    EmailAddress = string.Format("nas.exemplar+{0}@gmail.com", Guid.NewGuid()),
                    DateOfBirth = new DateTime(1980, 06, 15),
                    PhoneNumber = "01221234567",
                    Address = new Address
                    {
                        AddressLine1 = "103 Crawley Drive",
                        AddressLine3 = "Hemel Hempstead",
                        AddressLine4 = "Hertfordhsire",
                        Postcode = "HP2 6AL",
                        AddressLine2 = "Hemel Hempstead"
                    },
                }
            };

            var result = _legacyCandidateProvider.CreateCandidate(candidate);

            result.Should().BeGreaterThan(0);
        }
    }
}