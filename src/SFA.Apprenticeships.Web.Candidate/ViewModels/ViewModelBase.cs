namespace SFA.Apprenticeships.Web.Candidate.ViewModels
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(string message)
        {
            ViewModelMessage = message;
        }

        public string ViewModelMessage { get; private set; }

        public bool HasError()
        {
            return !string.IsNullOrWhiteSpace(ViewModelMessage);
        }
    }
}