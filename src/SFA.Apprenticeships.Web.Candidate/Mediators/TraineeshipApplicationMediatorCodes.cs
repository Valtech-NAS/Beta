namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class TraineeshipApplication
        {
            public class Apply
            {
                public const string Ok = "ok";
                public const string HasError = "error";
            }

            public class Submit
            {
                public const string IncorrectState = "incorrectState";
                public const string Error = "error";
                public const string Ok = "ok";
            }

            public class AddEmptyQualificationRows
            {
                public const string Ok = "ok";
            }

            public class AddEmptyWorkExperienceRows
            {
                public const string Ok = "ok";
            }

            public class WhatHappensNext
            {
                public const string VacancyNotFound = "vacancyNotFound";
                public const string Ok = "ok";
            }
        }
    }
}