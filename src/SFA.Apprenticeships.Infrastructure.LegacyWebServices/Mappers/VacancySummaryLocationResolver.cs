using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using VacancySummaryProxy;
    using Domain.Entities.Locations;

    public class VacancySummaryLocationResolver : ValueResolver<AddressData, GeoPoint>
    {
        protected override GeoPoint ResolveCore(AddressData source)
        {
            var point = new GeoPoint();

            if (source != null)
            {
                point.Latitude = (double) source.Latitude.GetValueOrDefault();
                point.Longitude = (double) source.Longitude.GetValueOrDefault();
            }

            return point;
        }
    }
}
