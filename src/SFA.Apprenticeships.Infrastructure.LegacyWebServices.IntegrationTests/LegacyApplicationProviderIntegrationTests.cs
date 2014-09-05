namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using Application.Candidate.Strategies;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Helpers;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LegacyApplicationProviderIntegrationTests : ICandidateReadRepository
    {
        private ILegacyApplicationProvider _legacyApplicationProviderProvider;
        private ILegacyCandidateProvider _legacyCandidateProvider;

        public Candidate Get(Guid id)
        {
            var candidate = new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            };

            return candidate;
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            throw new NotImplementedException();
        }

        public Candidate Get(string username, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.For<ICandidateReadRepository>().Use(this);
            });

            _legacyApplicationProviderProvider = ObjectFactory.GetInstance<ILegacyApplicationProvider>();
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
        }

        [Test]
        public void ShouldCreateApplication()
        {
            var applicationDetail = TestApplicationHelper.CreateFakeApplicationDetail();
            var result = _legacyApplicationProviderProvider.CreateApplication(applicationDetail);

            result.Should().BeGreaterThan(0);
        }

        [Test]
        public void ShouldCreateApplicationForCandidateWithNoInformation()
        {
            var applicationDetail = TestApplicationHelper.CreateFakeMinimalApplicationDetail();

            var result = _legacyApplicationProviderProvider.CreateApplication(applicationDetail);

            result.Should().BeGreaterThan(0);
        }

        private int CreateLegacyCandidateId()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "01683200911",
                    Address = new Address
                    {
                        AddressLine1 = "10 Acacia Avenue",
                        AddressLine3 = "Some House",
                        AddressLine4 = "Some Town",
                        Postcode = "FF2 7AL",
                        AddressLine2 = "East Nether"
                    },
                }
            };

            return _legacyCandidateProvider.CreateCandidate(candidate);
        }
    }
}