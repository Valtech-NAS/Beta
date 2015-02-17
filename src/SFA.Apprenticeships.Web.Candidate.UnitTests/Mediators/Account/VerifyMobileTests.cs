
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;

   public class VerifyMobileTests
    {
       private const string MobileNumber = "123456789";
       private const string ValidVerificationCode = "1234";
       private const string InvalidVerificationCode = "987654321";

       [TestCase(VerifyMobileState.Ok, AccountMediatorCodes.VerifyMobile.Success, VerifyMobilePageMessages.MobileVerificationSuccessText, UserMessageLevel.Success)]
       [TestCase(VerifyMobileState.MobileVerificationNotRequired, AccountMediatorCodes.VerifyMobile.VerificationNotRequired, VerifyMobilePageMessages.MobileVerificationNotRequired, UserMessageLevel.Warning)]
       [TestCase(VerifyMobileState.Error, AccountMediatorCodes.VerifyMobile.Error, VerifyMobilePageMessages.MobileVerificationError, UserMessageLevel.Error)]
       [TestCase(VerifyMobileState.VerifyMobileCodeInvalid, AccountMediatorCodes.VerifyMobile.InvalidCode, VerifyMobilePageMessages.MobileVerificationCodeInvalid, UserMessageLevel.Error)]
       public void VerifyMobileTest(VerifyMobileState verifyMobileState, string accountMediatorCode, string pageMessage, UserMessageLevel userMessageLevel)
        {
            //Arrange
            //const VerifyMobileState verifyMobileState = VerifyMobileState.Ok;
            var verifyMobileViewModel = new VerifyMobileViewModelBuilder(MobileNumber, ValidVerificationCode, verifyMobileState).Build();

            var verifyMobileViewModelServerValidatorMock = new Mock<VerifyMobileViewModelServerValidator>();
            verifyMobileViewModelServerValidatorMock.Setup(x => x.Validate(It.IsAny<VerifyMobileViewModel>())).Returns(new ValidationResult());

            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.VerifyMobile(It.IsAny<Guid>(), It.IsAny<VerifyMobileViewModel>())).Returns(verifyMobileViewModel);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).With(verifyMobileViewModelServerValidatorMock).Build();

            //Act
            var response = accountMediator.VerifyMobile(Guid.NewGuid(), new VerifyMobileViewModel() { PhoneNumber = MobileNumber, VerifyMobileCode = ValidVerificationCode });

            //Assert
            response.Code.Should().Be(accountMediatorCode);
            response.Message.Text.Should().Be(pageMessage);
            response.Message.Level.Should().Be(userMessageLevel);
        }
        

       [Test]
        public void ValidationErrorTest()
        {
            //Arrange
            VerifyMobileState verifyMobileState = VerifyMobileState.VerifyMobileCodeInvalid;
            var v = new ValidationResult();
            v.Errors.Add(new ValidationFailure("VerifyMobileCode", "Length should be less than 4 digits"));

            var verifyMobileViewModel = new VerifyMobileViewModelBuilder(MobileNumber, InvalidVerificationCode, verifyMobileState).Build();

           //todo: nice to have to convert into a builder class
            var verifyMobileViewModelServerValidatorMock = new Mock<VerifyMobileViewModelServerValidator>();
            verifyMobileViewModelServerValidatorMock.Setup(x => x.Validate(It.IsAny<VerifyMobileViewModel>())).Returns(v);

            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.VerifyMobile(It.IsAny<Guid>(), It.IsAny<VerifyMobileViewModel>())).Returns(verifyMobileViewModel);

            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).With(verifyMobileViewModelServerValidatorMock).Build();

            //Act
            var response = accountMediator.VerifyMobile(Guid.NewGuid(), new VerifyMobileViewModel() { PhoneNumber = MobileNumber, VerifyMobileCode = ValidVerificationCode });

            //Assert
            response.Code.Should().Be(AccountMediatorCodes.VerifyMobile.ValidationError);
            response.ValidationResult.Should().NotBeNull();
        }
    }
}
