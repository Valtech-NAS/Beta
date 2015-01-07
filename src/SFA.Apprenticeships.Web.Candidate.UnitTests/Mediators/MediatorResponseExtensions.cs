namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators
{
    using Candidate.Mediators;
    using Common.Constants;
    using FluentAssertions;

    public static class MediatorResponseExtensions
    {
        public static void AssertCode<T>(this MediatorResponse<T> response, string code, bool viewModelShouldNotBeNull, bool parametersShouldNotBeNull = false)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Should().BeNull();
            response.AssertParameters(parametersShouldNotBeNull);
            response.ValidationResult.Should().BeNull();
        }

        public static void AssertMessage<T>(this MediatorResponse<T> response, string code, string message, UserMessageLevel messageLevel, bool viewModelShouldNotBeNull, bool parametersShouldNotBeNull = false)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Text.Should().Be(message);
            response.Message.Level.Should().Be(messageLevel);
            response.AssertParameters(parametersShouldNotBeNull);
            response.ValidationResult.Should().BeNull();
        }

        public static void AssertValidationResult<T>(this MediatorResponse<T> response, string code, bool viewModelShouldNotBeNull, bool parametersShouldNotBeNull = false)
        {
            response.Code.Should().Be(code);
            response.AssertViewModel(viewModelShouldNotBeNull);
            response.Message.Should().BeNull();
            response.AssertParameters(parametersShouldNotBeNull);
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

        private static void AssertParameters<T>(this MediatorResponse<T> response, bool parametersShouldNotBeNull)
        {
            if (parametersShouldNotBeNull)
            {
                response.Parameters.Should().NotBeNull();
            }
            else
            {
                response.Parameters.Should().BeNull();
            }
        }
    }
}