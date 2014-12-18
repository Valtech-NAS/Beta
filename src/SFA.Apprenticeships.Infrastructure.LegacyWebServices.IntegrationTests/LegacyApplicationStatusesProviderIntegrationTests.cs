namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using Application.ApplicationUpdate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Helpers;
    using IoC;
    using Moq;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using StructureMap;

    [TestFixture]
    public class LegacyApplicationStatusesProviderIntegrationTests
    {
        private const int TestVacancyId = 445650;

        private ILegacyCandidateProvider _legacyCandidateProvider;
        private ILegacyApplicationProvider _legacyApplicationProvider;
        private ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;

        private readonly Mock<ICandidateReadRepository> _candidateReadRepositoryMock = new Mock<ICandidateReadRepository>();

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.For<ICandidateReadRepository>().Use(_candidateReadRepositoryMock.Object);
            });

            // Providers.
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
            _legacyApplicationProvider = ObjectFactory.GetInstance<ILegacyApplicationProvider>();
            _legacyApplicationStatusesProvider = ObjectFactory.GetInstance<ILegacyApplicationStatusesProvider>();
#pragma warning restore 0618
        }

        [Test, Category("Integration")]
        public void ShouldNotGetAnyApplicationStatusesForCandidateWithNoSubmittedApplications()
        {
            // Arrange.
            var candidate = CreateCandidate();
            _candidateReadRepositoryMock.ResetCalls();
            _candidateReadRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(candidate);

            // Act.
            var result = _legacyApplicationStatusesProvider
                .GetCandidateApplicationStatuses(candidate)
                .ToList();

            // Assert.
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Test, Category("Integration")]
        public void ShouldGetOneApplicationStatusForCandidateWithOneSubmittedApplication()
        {
            // Arrange.
            var candidate = CreateCandidate();
            _candidateReadRepositoryMock.ResetCalls();
            _candidateReadRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(candidate);

            var applicationDetail = CreateApplicationForCandidate(candidate);

            // Act: get application statuses (should only be one).
            var statuses = _legacyApplicationStatusesProvider
                .GetCandidateApplicationStatuses(candidate)
                .ToList();

            // Assert.
            statuses.Should().NotBeNull();
            statuses.Count().Should().Be(1);

            var status = statuses.First();

            status.ApplicationStatus.Should().Be(ApplicationStatuses.Submitted);
            status.LegacyApplicationId.Should().Be(applicationDetail.LegacyApplicationId);
        }

        private Candidate CreateCandidate()
        {
            // Create a new candidate in legacy and repo.
            var candidate = TestCandidateHelper.CreateFakeCandidate();

            candidate.LegacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);

            return candidate;
        }

        private ApprenticeshipApplicationDetail CreateApplicationForCandidate(Candidate candidate)
        {
            // Create a new application for candidate in legacy and repo.
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            applicationDetail.CandidateId = candidate.EntityId;

            var legacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);

            applicationDetail.LegacyApplicationId = legacyApplicationId;

            return applicationDetail;
        }
    }
}