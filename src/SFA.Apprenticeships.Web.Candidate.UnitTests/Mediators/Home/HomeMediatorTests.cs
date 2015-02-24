namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Home
{
    using System;
    using Candidate.Mediators;
    using Candidate.Mediators.Home;
    using Candidate.Providers;
    using Candidate.ViewModels.Home;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HomeMediatorTests : MediatorBase
    {
        private readonly Mock<ICandidateServiceProvider> _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
        private readonly Mock<IHomeProvider> _homeProviderMock = new Mock<IHomeProvider>();
        private const string AString = "A string";
            
        [Test]
        public void GetContactMessageViewModelWithoutCandidateId()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _homeProviderMock.Object);

            var response = homeMediator.GetContactMessageViewModel(null);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void GetContactMessageViewModelWithCandidateId()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _homeProviderMock.Object);
            var candidateId = Guid.NewGuid();
            const string candidateFirstName = "John";
            const string candidateLastName = "Doe";
            const string emailAddress = "someemail@gmail.com";
            
            _candidateServiceProviderMock.Setup(csp => csp.GetCandidate(candidateId)).Returns(new Candidate
            {
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = candidateFirstName,
                    LastName = candidateLastName,
                    EmailAddress = emailAddress
                }
            });

            var response = homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().Be(string.Format("{0} {1}", candidateFirstName, candidateLastName));
            response.ViewModel.Email.Should().Be(emailAddress);
        }

        [Test]
        public void GetContactMessageViewModelWithoError()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _homeProviderMock.Object);
            var candidateId = Guid.NewGuid();
            _candidateServiceProviderMock.Setup(csp => csp.GetCandidate(candidateId)).Throws<ArgumentException>();

            var response = homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void SendContactMessage()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _homeProviderMock.Object);
            var viewModel = new ContactMessageViewModel
            {
                Details = AString,
                Email = AString,
                Enquiry = AString,
                Name = AString,
                SelectedEnquiry = AString
            };

            _homeProviderMock.Setup(hp => hp.SendContactMessage(null, viewModel)).Returns(true);
            var response = homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success, true);
        }

        [Test]
        public void SendContactMessageFail()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _homeProviderMock.Object);
            var viewModel = new ContactMessageViewModel
            {
                Details = AString,
                Email = AString,
                Enquiry = AString,
                Name = AString,
                SelectedEnquiry = AString
            };

            _homeProviderMock.Setup(hp => hp.SendContactMessage(null, viewModel)).Returns(false);
            var response = homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.Error,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning, true);
        }
    }
}