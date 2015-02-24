namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Home
{
    using System;
    using Application.Interfaces.Candidates;
    using Candidate.Providers;
    using Candidate.ViewModels.Home;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Mapping;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HomeProviderTests
    {
        private Mock<ICandidateService> _candidateServiceMock;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        [SetUp]
        public void SetUp()
        {
            _candidateServiceMock = new Mock<ICandidateService>();
        }

        [Test]
        public void SendMessageOk()
        {
            var candidateId = Guid.NewGuid();

            var homeProvider = new HomeProvider(_candidateServiceMock.Object, _mapperMock.Object);
            _mapperMock.Setup(m => m.Map<ContactMessageViewModel, ContactMessage>(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ContactMessage());

            var result = homeProvider.SendContactMessage(candidateId, new ContactMessageViewModel());

            result.Should().BeTrue();
            _candidateServiceMock.Verify(cs => cs.SendContactMessage(It.Is<ContactMessage>(cm => cm.UserId == candidateId)));
        }

        [Test]
        public void SendMessageFail()
        {
            var candidateId = Guid.NewGuid();

            var homeProvider = new HomeProvider(_candidateServiceMock.Object, _mapperMock.Object);
            _mapperMock.Setup(m => m.Map<ContactMessageViewModel, ContactMessage>(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ContactMessage());

            _candidateServiceMock.Setup(cs => cs.SendContactMessage(It.IsAny<ContactMessage>()))
                .Throws<ArgumentException>();

            var result = homeProvider.SendContactMessage(candidateId, new ContactMessageViewModel());

            result.Should().BeFalse();
        }
    }
}