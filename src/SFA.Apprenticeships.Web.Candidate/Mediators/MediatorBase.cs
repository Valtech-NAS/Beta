namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using Common.Constants;

    public abstract class MediatorBase
    {
        protected static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel)
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel
            };

            return response;
        }

        protected static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel, string message, UserMessageLevel level)
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                Message = new MediatorResponseMessage
                {
                    Message = message,
                    Level = level
                }
            };

            return response;
        }
    }
}