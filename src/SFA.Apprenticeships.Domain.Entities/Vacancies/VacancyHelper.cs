namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System.Text.RegularExpressions;

    public class VacancyHelper
    {
        private static readonly Regex VacancyReferenceNumberRegex = new Regex(@"^(VAC)?(\d{6,9})");

        public static bool IsVacancyReferenceNumber(string value)
        {
            return !string.IsNullOrEmpty(value) && VacancyReferenceNumberRegex.IsMatch(value);
        }

        public static bool TryGetVacancyReferenceNumber(string value, out int vacancyReferenceNumber)
        {
            vacancyReferenceNumber = 0;
            
            if(string.IsNullOrEmpty(value))
                return false;
            
            var match = VacancyReferenceNumberRegex.Match(value);
            
            if (match.Success)
            {
                var vacancyReferenceNumberString = match.Groups[2].Value;
                vacancyReferenceNumber = int.Parse(vacancyReferenceNumberString);
            }
            
            return match.Success;
        }
    }
}