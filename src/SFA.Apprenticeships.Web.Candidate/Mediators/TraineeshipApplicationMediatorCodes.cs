namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class TraineeshipApplication
        {
            public class Apply
            {
                public const string VacancyNotFound = "TraineeshipApplication.Apply.VacancyNotFound";
                public const string Ok = "TraineeshipApplication.Apply.Ok";
                public const string HasError = "TraineeshipApplication.Apply.HasError";
            }

            public class Submit
            {
                public const string IncorrectState = "TraineeshipApplication.Submit.IncorrectState";
                public const string Error = "TraineeshipApplication.Submit.Error";
                public const string Ok = "TraineeshipApplication.Submit.Ok";
            }

            public class AddEmptyQualificationRows
            {
                public const string Ok = "TraineeshipApplication.AddEmptyQualificationRows.Ok";
            }

            public class AddEmptyWorkExperienceRows
            {
                public const string Ok = "TraineeshipApplication.AddEmptyWorkExperienceRows.Ok";
            }

            public class WhatHappensNext
            {
                public const string VacancyNotFound = "TraineeshipApplication.WhatHappensNext.VacancyNotFound";
                public const string Ok = "TraineeshipApplication.WhatHappensNext.Ok";
            }
        }
    }
}