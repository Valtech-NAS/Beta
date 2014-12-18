namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class VacancyLocationTypeResolver : ValueResolver<string, ApprenticeshipLocationType>
    {
        protected override ApprenticeshipLocationType ResolveCore(string source)
        {
            if (source == null)
            {
                return ApprenticeshipLocationType.Unknown;
            }
           
            switch (source)
            {
                case "National":              
                    return ApprenticeshipLocationType.National;
                case "Standard":
                case "MultipleLocation":               
                    // TODO: DONTKNOW: MultiLocation are posted once for each location so are equivelent to NonNational but needs backed by requirements in new system.
                    return ApprenticeshipLocationType.NonNational;
                default:
                    throw new ArgumentException(
                        string.Format(
                            "The vacancy location is not valid: {0}, it must be either National or Standard to map correctly",
                            source));
            }
        }
    }
}
