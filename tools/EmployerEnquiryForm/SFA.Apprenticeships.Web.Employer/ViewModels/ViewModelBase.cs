namespace SFA.Apprenticeships.Web.Employer.ViewModels
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
        
        public string ViewModelMessage { get; set; }
        
        public bool HasError()
        {
            return !string.IsNullOrWhiteSpace(ViewModelMessage);
        }
    }
}