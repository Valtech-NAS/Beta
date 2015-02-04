namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class WageTypeResolver : ValueResolver<string, WageType>
    {
        protected override WageType ResolveCore(string source)
        {
            switch (source)
            {
                case "Weekly":
                    return WageType.Weekly;

                case "Text":
                    return WageType.Text;

                default:
                    throw new ArgumentOutOfRangeException("source",
                        string.Format("Unknown Wage Type received from NAS Gateway Service: \"{0}\"", source));
            }
        }
    }
}