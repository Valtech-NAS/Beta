namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System.Linq;
    using Application.ApplicationUpdate;
    using Application.Candidate.Strategies;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Helpers;
    using IoC;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using StructureMap;

    [TestFixture]
    public class LegacyApplicationStatusesProviderIntegrationTests
    {
        private ILegacyCandidateProvider _legacyCandidateProvider;
        private ILegacyApplicationProvider _legacyApplicationProvider;
        private ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;

        private ICandidateWriteRepository _candidateWriteRepository;
        private IApplicationWriteRepository _applicationWriteRepository;

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
            });

            // Providers.
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
            _legacyApplicationProvider = ObjectFactory.GetInstance<ILegacyApplicationProvider>();
            _legacyApplicationStatusesProvider = ObjectFactory.GetInstance<ILegacyApplicationStatusesProvider>();

            // Repositories.
            _candidateWriteRepository = ObjectFactory.GetInstance<ICandidateWriteRepository>();
            _applicationWriteRepository = ObjectFactory.GetInstance<IApplicationWriteRepository>();
        }

        [Test, Category("Integration")]
        public void ShouldNotGetAnyApplicationStatusesForCandidateWithNoSubmittedApplications()
        {
            // Arrange.
            var candidate = CreateCandidate();

            // Act.
            var result = _legacyApplicationStatusesProvider
                .GetCandidateApplicationStatuses(candidate)
                .ToList();

            // Assert.
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Test, Category("Integration"), Ignore]
        public void ShouldGetOneApplicationStatusForCandidateWithOneSubmittedApplication()
        {
            // Arrange.
            var candidate = CreateCandidate();
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
            candidate = _candidateWriteRepository.Save(candidate);

            return candidate;
        }

        private ApplicationDetail CreateApplicationForCandidate(Candidate candidate)
        {
            // Create a new application for candidate in legacy and repo.
            var applicationDetail = TestApplicationHelper.CreateFakeApplicationDetail();

            applicationDetail.CandidateId = candidate.EntityId;
            applicationDetail = _applicationWriteRepository.Save(applicationDetail);

            var legacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);

            applicationDetail.LegacyApplicationId = legacyApplicationId;

            return applicationDetail;
        }
    }
}