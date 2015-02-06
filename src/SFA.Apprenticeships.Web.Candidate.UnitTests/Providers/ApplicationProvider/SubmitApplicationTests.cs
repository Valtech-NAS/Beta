namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitApplicationTests : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenViewModelHasError_ThenItIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var viewModel = new ApprenticeshipApplicationViewModelBuilder()
                .HasError(ApplicationStatuses.ExpiredOrWithdrawn, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable)
                .Build();

            var returnedViewModel = ApprenticeshipApplicationProvider.SubmitApplication(candidateId, ValidVacancyId);
        }
    }
}