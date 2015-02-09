namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveApplicationTests
    {
        const int ValidVacancyId = 1;

        [Test]
        public void GivenSaveException_ThenExceptionIsThrown()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.SaveApplication(candidateId, ValidVacancyId, null)).Throws<Exception>();

            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).Build()
                .SaveApplication(candidateId, ValidVacancyId, null);

            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenValidViewModel_ThenSaveIsCalled()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var viewModel = new ApprenticeshipApplicationViewModelBuilder().Build();

            new ApprenticeshipApplicationProviderBuilder().With(candidateService).Build().SaveApplication(candidateId, ValidVacancyId, viewModel);
            
            candidateService.Verify(cs => cs.SaveApplication(candidateId, ValidVacancyId, It.IsAny<ApprenticeshipApplicationDetail>()), Times.Once);
        }
    }
}