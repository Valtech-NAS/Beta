namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Account;
    using CandidateServiceProvider;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using FluentValidation.Results;
    using Mediators.Account;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyMobileTests
    {
        private const string MobileNumber = "123456789";
        private const string ValidVerificationCode = "1234";
        private const string InvalidVerificationCode = "987654321";

        //[TestCase(VerifyMobileState.Ok)]
        //[TestCase(VerifyMobileState.MobileVerificationNotRequired)]
        //[TestCase(VerifyMobileState.Error)]
        //[TestCase(VerifyMobileState.VerifyMobileCodeInvalid)]
        //public void VerifyMobileTest(VerifyMobileState verifyMobileState)
        //{
        //    //Arrange
        //    var candidateServiceMock = new Mock<ICandidateService>();
        //    candidateServiceMock.Setup(r => r.VerifyMobileCode(It.IsAny<Guid>(), It.IsAny<string>()));
            
        //    var accountProvider = new AccountProviderBuilder().With(candidateServiceMock).Build();

        //    //Act
        //    var response = accountProvider.VerifyMobile(Guid.NewGuid(), new VerifyMobileViewModel() { PhoneNumber = MobileNumber, VerifyMobileCode = ValidVerificationCode });

        //    //Assert
        //    response.Status.Should().Be(verifyMobileState);
        //}
    }
}