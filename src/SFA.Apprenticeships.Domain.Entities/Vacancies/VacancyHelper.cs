namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System.Text.RegularExpressions;

    public class VacancyHelper
    {
        private static readonly Regex VacancyReferenceNumberRegex = new Regex(@"^(VAC)?(\d{6,9})");

        public static bool IsVacancyReference(string value)
        {
            return !string.IsNullOrEmpty(value) && VacancyReferenceNumberRegex.IsMatch(value);
        }

        public static bool TryGetVacancyReference(string value, out string vacancyReference)
        {
            vacancyReference = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            var match = VacancyReferenceNumberRegex.Match(value);
            
            if (match.Success)
            {
                vacancyReference = match.Groups[2].Value;
                int vacancyReferenceNumber;
                if (!string.IsNullOrEmpty(vacancyReference) && int.TryParse(vacancyReference, out vacancyReferenceNumber))
                {
                    vacancyReference = vacancyReferenceNumber.ToString();
                }
            }

            return match.Success;
        }
    }
}