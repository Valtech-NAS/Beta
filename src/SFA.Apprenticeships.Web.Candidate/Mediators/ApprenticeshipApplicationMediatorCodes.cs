namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class ApprenticeshipApplication
        {
            public class Apply
            {
                public const string Ok = "ApprenticeshipApplication.Apply.Ok";
                public const string HasError = "ApprenticeshipApplication.Apply.HasError";
                public const string VacancyNotFound = "ApprenticeshipApplication.Apply.VacancyNotFound";
            }

            public class Submit
            {
                public const string VacancyNotFound = "ApprenticeshipApplication.Submit.VacancyNotFound";
                public const string IncorrectState = "ApprenticeshipApplication.Submit.IncorrectState";
                public const string Error = "ApprenticeshipApplication.Submit.Error";
                public const string Ok = "ApprenticeshipApplication.Submit.Ok";
                public const string ValidationError = "ApprenticeshipApplication.Submit.ValidationError";
            }

            public class AddEmptyQualificationRows
            {
                public const string Ok = "ApprenticeshipApplication.AddEmptyQualificationRows.Ok";
            }

            public class AddEmptyWorkExperienceRows
            {
                public const string Ok = "ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok";
            }

            public class WhatHappensNext
            {
                public const string VacancyNotFound = "ApprenticeshipApplication.WhatHappensNext.VacancyNotFound";
                public const string Ok = "ApprenticeshipApplication.WhatHappensNext.Ok";
            }

            public class Resume
            {
                public const string Ok = "ApprenticeshipApplication.Resume.Ok";
                public const string HasError = "ApprenticeshipApplication.Resume.HasError";
            }

            public class PreviewAndSubmit
            {
                public const string VacancyNotFound = "ApprenticeshipApplication.PreviewAndSubmit.VacancyNotFound";
                public const string IncorrectState = "ApprenticeshipApplication.PreviewAndSubmit.IncorrectState";
                public const string Error = "ApprenticeshipApplication.PreviewAndSubmit.Error";
                public const string ValidationError = "ApprenticeshipApplication.PreviewAndSubmit.ValidationError";
                public const string Ok = "ApprenticeshipApplication.PreviewAndSubmit.Ok";
            }

            public class Preview
            {
                public const string Ok = "ApprenticeshipApplication.Preview.Ok";
                public const string HasError = "ApprenticeshipApplication.Preview.HasError";
                public const string VacancyNotFound = "ApprenticeshipApplication.Preview.VacancyNotFound";
            }

            public class Save
            {
                public const string VacancyNotFound = "ApprenticeshipApplication.Save.VacancyNotFound";
                public const string Error = "ApprenticeshipApplication.Save.Error";
                public const string ValidationError = "ApprenticeshipApplication.Save.ValidationError";
                public const string Ok = "ApprenticeshipApplication.Save.Ok";
            }

            public class AutoSave
            {
                public const string VacancyNotFound = "ApprenticeshipApplication.AutoSave.VacancyNotFound";
                public const string ValidationError = "ApprenticeshipApplication.AutoSave.ValidationError";
                public const string HasError = "ApprenticeshipApplication.AutoSave.HasError";
                public const string Ok = "ApprenticeshipApplication.AutoSave.Ok";
            }
        }
    }
}