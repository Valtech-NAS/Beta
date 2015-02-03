namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class ApprenticeshipApplication
        {
            public class Apply
            {
                public const string Ok = "ok";
                public const string HasError = "error";
                public const string VacancyNotFound = "vacancyNotFound";
            }

            public class Submit
            {
                public const string VacancyNotFound = "vacancyNotFound";
                public const string IncorrectState = "incorrectState";
                public const string Error = "error";
                public const string Ok = "ok";
                public const string ValidationError = "validationError";
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

            public class Resume
            {
                public const string Ok = "ok";
                public const string HasError = "error";
            }

            public class PreviewAndSubmit
            {
                public const string VacancyNotFound = "vacancyNotFound";
                public const string IncorrectState = "incorrectState";
                public const string Error = "error";
                public const string ValidationError = "validationError";
                public const string Ok = "ok";
            }

            public class Preview
            {
                public const string Ok = "ok";
                public const string HasError = "error";
                public const string VacancyNotFound = "vacancyNotFound";
            }

            public class Save
            {
                public const string VacancyNotFound = "vacancyNotFound";
                public const string Error = "error";
                public const string ValidationError = "validationError";
                public const string Ok = "ok";
            }

            public class AutoSave
            {
                public const string VacancyNotFound = "vacancyNotFound";
                public const string ValidationError = "validationError";
                public const string HasError = "error";
                public const string Ok = "ok";
            }
        }
    }
}