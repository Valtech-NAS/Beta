namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using Application.Candidate;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Helpers;
    using IoC;
    using Logging.IoC;
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
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.For<ICandidateReadRepository>().Use(_candidateRepositoryMock.Object);
            });

            _legacyApplicationProviderProvider = container.GetInstance<ILegacyApplicationProvider>();
            _legacyCandidateProvider = container.GetInstance<ILegacyCandidateProvider>();
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

        [Test, Category("Integration"), Category("SmokeTests")]
        [ExpectedException(Handler = "CheckForLegacyCandidateNotFoundErrorException")]
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
        public void ShouldGetAnErrorWhenCreatinganApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder().Build();

            applicationDetail.CandidateInformation.EducationHistory = new Education
            {
                Institution =  "GENERAL_ERROR",
                FromYear = 1999,
                ToYear = 2001
            };
          
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
            //_candidateRepositoryMock.ResetCalls();
            //_candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(new Candidate
            //{
            //    LegacyCandidateId = CreateLegacyCandidateId()
            //});
            //var applicationDetail = new TestApplicationBuilder().Build();

            //applicationDetail.CandidateInformation.EducationHistory = new Education()
            //{
            //    Institution = "GENERAL_ERROR",
            //    FromYear = 1999,
            //    ToYear = 2001
            //};
            

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        public void CheckForDuplicatedApplicationException( Exception ex )
        {
            ex.Should().BeOfType<CustomException>();

            var cex = ex as CustomException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(Application.Interfaces.Applications.ErrorCodes.ApplicationDuplicatedError);
        }

        public void CheckForApplicationGatewayCreationException(Exception ex)
        {
            ex.Should().BeOfType<CustomException>();

            var cex = ex as CustomException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(ErrorCodes.ApplicationGatewayCreationError);
        }

        public void CheckForLegacyCandidateNotFoundErrorException(Exception ex)
        {
            ex.Should().BeOfType<CustomException>();

            var cex = ex as CustomException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(Application.Interfaces.Candidates.ErrorCodes.LegacyCandidateNotFoundError);
        }

        private int CreateLegacyCandidateId()
        {
            var candidate = TestCandidateHelper.CreateFakeCandidate();

            return _legacyCandidateProvider.CreateCandidate(candidate);
        }
    }
}