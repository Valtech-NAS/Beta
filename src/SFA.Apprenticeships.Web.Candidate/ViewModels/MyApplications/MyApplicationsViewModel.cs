namespace SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;

    public class MyApplicationsViewModel
    {
        public MyApplicationsViewModel(
            IEnumerable<MyApprenticeshipApplicationViewModel> apprenticeshipApplications,
            IEnumerable<MyTraineeshipApplicationViewModel> traineeshipApplications, 
            TraineeshipPromptViewModel traineeshipPrompt)
        {
            AllApprenticeshipApplications = apprenticeshipApplications
                .Where(a => !a.IsArchived)
                .OrderByDescending(a => a.DateUpdated);

            TraineeshipApplications = traineeshipApplications
                .Where(a => !a.IsArchived)
                .OrderByDescending(a => a.DateApplied);

            TraineeshipPrompt = traineeshipPrompt;
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> AllApprenticeshipApplications { get; private set; }

        public IOrderedEnumerable<MyTraineeshipApplicationViewModel> TraineeshipApplications { get; private set; }

        public TraineeshipPromptViewModel TraineeshipPrompt { get; set; }

        public IEnumerable<MyApprenticeshipApplicationViewModel> SubmittedApprenticeshipApplications
        {
            get
            {
                return AllApprenticeshipApplications.Where(each =>
                    each.ApplicationStatus == ApplicationStatuses.Submitting ||
                    each.ApplicationStatus == ApplicationStatuses.Submitted);
            }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> SuccessfulApprenticeshipApplications
        {
            get { return AllApprenticeshipApplications.Where(each => each.ApplicationStatus == ApplicationStatuses.Successful); }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> UnsuccessfulApplications
        {
            get
            {
                return AllApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Unsuccessful ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && each.DateApplied.HasValue));
            }
        }

        public IEnumerable<MyApprenticeshipApplicationViewModel> DraftApprenticeshipApplications
        {
            get
            {
                // Return Draft or Expired / Withdrawn draft applications.
                return AllApprenticeshipApplications
                    .Where(each =>
                        each.ApplicationStatus == ApplicationStatuses.Draft ||
                        (each.ApplicationStatus == ApplicationStatuses.ExpiredOrWithdrawn && !each.DateApplied.HasValue))
                     .OrderBy(app => app.ClosingDate);
            }
        }

        public bool ShowTraineeshipsPrompt
        {
            get
            {
                // Candidate has a number of unsuccessful apprenticeship applications and has not applied for a traineeship yet.
                return TraineeshipPrompt.TraineeshipsFeatureActive &&
                        TraineeshipPrompt.AllowTraineeshipPrompts &&
                       AllApprenticeshipApplications.Count(each => each.ApplicationStatus == ApplicationStatuses.Unsuccessful) >= TraineeshipPrompt.UnsuccessfulApplicationsToShowTraineeshipsPrompt
                       && !TraineeshipApplications.Any();
            }
        }

        public string DeletedVacancyId { get; set; }

        public string DeletedVacancyTitle { get; set; }
    }
}
