namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
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
                    return WageType.Unknown;
            }
        }
    }
}