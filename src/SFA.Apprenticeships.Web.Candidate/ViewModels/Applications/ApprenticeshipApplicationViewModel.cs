namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using System.Configuration;
    using Domain.Entities.Vacancies;
    using FluentValidation.Attributes;
    using Domain.Entities.Applications;
    using Candidate;
    using Common.Models.Application;
    using Validators;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    [Serializable]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModelBase
    {
        //Constants used on application form
        public string AutoSaveTimeInMiutes = ConfigurationManager.AppSettings["AutoSaveTimeInMinutes"];

        public ApplicationStatuses Status { get; set; }

        public ApprenticeshipCandidateViewModel Candidate { get; set; }

        public ApprenticeshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public ApprenticeshipApplicationViewModel(string message) : base(message)
        {
        }

        public ApprenticeshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus) : base(viewModelStatus)
        {
        }

        public ApprenticeshipApplicationViewModel()
        {
        }

        //TODO: Think in a better name
        public bool IsExpiredOrWithdrawn()
        {
            return Status == ApplicationStatuses.ExpiredOrWithdrawn || VacancyDetail.VacancyStatus != VacancyStatuses.Live;
        }
    }
}