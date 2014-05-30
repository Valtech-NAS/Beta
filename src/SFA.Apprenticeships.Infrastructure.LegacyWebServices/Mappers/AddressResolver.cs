using SFA.Apprenticeships.Domain.Entities.Location;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;

    public class AddressResolver : ValueResolver<AddressData, GeoPoint>
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
