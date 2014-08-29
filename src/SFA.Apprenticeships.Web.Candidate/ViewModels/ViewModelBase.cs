namespace SFA.Apprenticeships.Web.Candidate.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ViewModelBase
    {
        public ViewModelBase() { }

        public ViewModelBase(string message)
        {
            ViewModelMessage = message;
        }

        public string ViewModelMessage { get; private set; }

        public bool NeedsToShowAUserMessage()
        {
            return !string.IsNullOrWhiteSpace(ViewModelMessage);
        }
    }
}
