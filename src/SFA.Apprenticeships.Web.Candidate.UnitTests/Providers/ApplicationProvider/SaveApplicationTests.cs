namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveApplicationTests : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenSaveException_ThenExceptionIsThrown()
        {
            var candidateId = Guid.NewGuid();
            CandidateService.Setup(cs => cs.SaveApplication(candidateId, ValidVacancyId, null)).Throws<Exception>();

            Action patchApplicationViewModelAction = () => ApprenticeshipApplicationProvider.SaveApplication(candidateId, ValidVacancyId, null);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenValidViewModel_ThenSaveIsCalled()
        {
            var candidateId = Guid.NewGuid();
            var viewModel = new ApprenticeshipApplicationViewModelBuilder().Build();

            ApprenticeshipApplicationProvider.SaveApplication(candidateId, ValidVacancyId, viewModel);
            CandidateService.Verify(cs => cs.SaveApplication(candidateId, ValidVacancyId, It.IsAny<ApprenticeshipApplicationDetail>()), Times.Once);
        }
    }
}