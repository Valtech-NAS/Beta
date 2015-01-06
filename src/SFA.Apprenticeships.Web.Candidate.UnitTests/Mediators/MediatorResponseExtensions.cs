namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators
{
    using Candidate.Mediators;
    using Common.Constants;
    using FluentAssertions;

    public static class MediatorResponseExtensions
    {
        public static void AssertCode<T>(this MediatorResponse<T> response, string code, bool viewModelShouldNotBeNull)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();
        }

        public static void AssertMessage<T>(this MediatorResponse<T> response, string code, string message, UserMessageLevel messageLevel, bool viewModelShouldNotBeNull)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Message.Should().Be(message);
            response.Message.Level.Should().Be(messageLevel);
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();
        }

        public static void AssertValidationResult<T>(this MediatorResponse<T> response, string code, bool viewModelShouldNotBeNull)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().NotBeNull();
        }

        private static void AssertViewModel<T>(this MediatorResponse<T> response, bool viewModelShouldNotBeNull)
        {
            if (viewModelShouldNotBeNull)
            {
                response.ViewModel.Should().NotBeNull();
            }
            else
            {
                response.ViewModel.Should().BeNull();
            }
        }
    }
}