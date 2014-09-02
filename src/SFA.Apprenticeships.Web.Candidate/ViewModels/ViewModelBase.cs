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

        // TODO: AG: consider refactoring into PageMessageViewModel with Text (string) and Level (UserMessageLevel).
        public string ViewModelMessage { get; set; }

        // TODO: AG: see above. Consider refactoring as HasPageMessage property (not function).
        public bool HasError()
        {
            return !string.IsNullOrWhiteSpace(ViewModelMessage);
        }
    }
}