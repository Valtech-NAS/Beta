using System;
using System.Globalization;
using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public class VacancySummaryResponse : VacancySummary
    {
        public double Distance { get; set; }

        //TODO: this needs tidied up and tested.
        public string DistanceAsString
        {
            get
            {
                return Math.Round(Distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
