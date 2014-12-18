namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using Domain.Entities.Applications;

    public class WhatHappensNextViewModel : ViewModelBase
    {
        public WhatHappensNextViewModel()
        {
        }

        public WhatHappensNextViewModel(string message)
            : base(message)
        {
        }

        public string VacancyTitle { get; set; }
        
        public string VacancyReference { get; set; }

        public ApplicationStatuses Status { get; set; }

        public bool SentEmail { get; set; }
    }
}
