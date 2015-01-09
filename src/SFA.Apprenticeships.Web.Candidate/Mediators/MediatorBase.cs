namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;
    using FluentValidation.Results;

    public abstract class MediatorBase
    {
        protected static MediatorResponse GetMediatorResponse(string code, ValidationResult validationResult = null, object parameters = null)
        {
            var response = new MediatorResponse
            {
                Code = code,
                ValidationResult = validationResult,
                Parameters = parameters
            };

            return response;
        }

        protected static MediatorResponse GetMediatorResponse(string code, string message, UserMessageLevel level, object parameters = null)
        {
            var response = new MediatorResponse
            {
                Code = code,
                Message = new MediatorResponseMessage
                {
                    Text = message,
                    Level = level
                },
                Parameters = parameters
            };

            return response;
        }

        protected static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel = default(T), ValidationResult validationResult = null, object parameters = null)
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                ValidationResult = validationResult,
                Parameters = parameters
            };

            return response;
        }

        protected static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel, string message, UserMessageLevel level, object parameters = null)
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                Message = new MediatorResponseMessage
                {
                    Text = message,
                    Level = level
                },
                Parameters = parameters
            };

            return response;
        }
    }
}