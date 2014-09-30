namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using Application.Candidate.Strategies;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Helpers;
    using IoC;
    using NUnit.Framework;
    using StructureMap;
    using Moq;
    using FluentAssertions;

    public class LegacyApplicationProviderIntegrationTests
    {
        private ILegacyApplicationProvider _legacyApplicationProviderProvider;
        private ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly Mock<ICandidateReadRepository> _candidateRepositoryMock = new Mock<ICandidateReadRepository>();

        private const int TestVacancyId = 445650;
        private const int CandidateId = int.MaxValue;

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<GatewayWebServicesRegistry>();
                x.For<ICandidateReadRepository>().Use(_candidateRepositoryMock.Object);
            });

            _legacyApplicationProviderProvider = ObjectFactory.GetInstance<ILegacyApplicationProvider>();
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
        }


        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForApplicationGatewayCreationException")]
        public void ShouldThrowAnErrorForanInvalidApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder().WithCandidateInformation().Build();
            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        public void ShouldCreateApplicationForAValidApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForApplicationGatewayCreationException")]
        public void ShouldThrowAnErrorIfTheCandidateDoesntExistInNasGateway()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CandidateId
            });
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForApplicationGatewayCreationException")]
        public void ShouldCreateApplicationForCandidateWithNoInformation()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder().Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForDuplicatedApplicationException")]
        public void ShouldGetACustomExceptionWhenResubmittingAnApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });

            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        public void CheckForDuplicatedApplicationException( Exception ex )
        {
            ex.Should().BeOfType<CustomException>();

            var cex = ex as CustomException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(ErrorCodes.ApplicationDuplicatedError);
        }

        public void CheckForApplicationGatewayCreationException(Exception ex)
        {
            ex.Should().BeOfType<CustomException>();

            var cex = ex as CustomException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(ErrorCodes.ApplicationGatewayCreationError);
        }

        private int CreateLegacyCandidateId()
        {
            var candidate = TestCandidateHelper.CreateFakeCandidate();

            return _legacyCandidateProvider.CreateCandidate(candidate);
        }
    }
}