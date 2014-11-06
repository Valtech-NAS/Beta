namespace SFA.Apprenticeships.Web.Candidate.Validators.Helpers
{
    using System;
    using ViewModels.Candidate;

    public class ValidatorsHelper
    {
        public static bool BeNowOrInThePast(string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                //Will be picked up by required validator
                return true;
            }

            int from;

            if (int.TryParse(year, out @from))
            {
                return @from <= DateTime.Now.Year;
            }

            return true;
        }

        public static bool BeNowOrInThePast(int? year)
        {
            if (year == null)
            {
                //Will be picked up by required validator
                return true;
            }

            return year.Value <= DateTime.Now.Year;
        }

        public static bool WorkExperienceYearBeBeforeOrEqual(WorkExperienceViewModel instance, string toYear)
        {
            if (string.IsNullOrEmpty(toYear))
            {
                //Will be picked up by required validator
                return true;
            }

            if (string.IsNullOrEmpty(instance.FromYear))
            {
                return false;
            }

            int to, from;

            var validTo = int.TryParse(toYear, out to);
            var validFrom = int.TryParse(instance.FromYear, out from);

            if (validTo && validFrom)
            {
                return @from <= to;
            }

            return true;
        }

        public static bool EducationYearBeBeforeOrEqual(EducationViewModel instance, int? toYear)
        {
            if (toYear==null)
            {
                //Will be picked up by required validator
                return true;
            }

            if (instance.FromYear == null )
            {
                return false;
            }

            return instance.FromYear < toYear;
        }
    }
}