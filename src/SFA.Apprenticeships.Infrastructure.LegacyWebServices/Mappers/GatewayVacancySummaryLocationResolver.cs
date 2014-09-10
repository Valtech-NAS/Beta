namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Locations;
    using GatewayServiceProxy;

    public class GatewayVacancySummaryLocationResolver : ValueResolver<VacancySummaryAddress, GeoPoint>
    {
        protected override GeoPoint ResolveCore(VacancySummaryAddress source)
        {
            var point = new GeoPoint();

            if (source != null)
            {
                point.Latitude = Convert.ToDouble(source.Latitude.GetValueOrDefault());
                point.Longitude = Convert.ToDouble(source.Longitude.GetValueOrDefault());
            }

            return point;
        }
    }
}
