namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApprenticeshipVacancyDetailProvider
{
    using System;
    using System.ServiceModel;
    using Application.Interfaces.Candidates;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetVacancyDetailViewModelTests
    {
        [Test]
        public void CandidateService_GetApprenticeshipVacancyDetail_ServerTooBusyException()
        {
            var candidateId = Guid.NewGuid();
            const int vacancyId = 1;
            var candidateService = new Mock<ICandidateService>();
            var message = string.Format("Get vacancy failed for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);
            var exception = new ServerTooBusyException("The HTTP service located at https://gateway.prod.avms.sfa.bis.gov.uk/services/NASGatewayService/GatewayService.svc is unavailable");
            var customException = new CustomException(message, exception, Application.Interfaces.Vacancies.ErrorCodes.GetVacancyDetailsFailed);
            candidateService.Setup(cs => cs.GetApprenticeshipVacancyDetail(candidateId, vacancyId)).Throws(customException);
            var provider = new ApprenticeshipVacancyDetailProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetVacancyDetailViewModel(candidateId, vacancyId);

            viewModel.ViewModelMessage.Should().Be(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
        }
    }
}