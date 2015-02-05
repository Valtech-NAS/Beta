namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System.Collections.Generic;
    using Candidate.ViewModels.MyApplications;
    using Domain.Entities.Applications;

    public static class DashboardTestsHelper
    {
        public static List<MyApprenticeshipApplicationViewModel> GetApprenticeships(int count,
    ApplicationStatuses applicationStatus = ApplicationStatuses.Draft)
        {
            var apprenticeships = new List<MyApprenticeshipApplicationViewModel>();

            for (var i = 0; i < count; i++)
            {
                apprenticeships.Add(new MyApprenticeshipApplicationViewModel
                {
                    ApplicationStatus = applicationStatus
                });
            }

            return apprenticeships;
        }

        public static List<MyTraineeshipApplicationViewModel> GetTraineeships(int count)
        {
            var traineeships = new List<MyTraineeshipApplicationViewModel>();

            for (var i = 0; i < count; i++)
            {
                traineeships.Add(new MyTraineeshipApplicationViewModel());
            }

            return traineeships;
        } 
    }
}