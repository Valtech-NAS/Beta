namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using GatewayServiceProxy;
    using Domain.Entities.Locations;

    public class LegacyVacancyDetailAddressDetailsResolver : ValueResolver<AddressDetails, Address>
    {
        protected override Address ResolveCore(AddressDetails source)
        {
            var address = new Address
            {
                GeoPoint = new GeoPoint()
            };

            if (source != null)
            {
                var addressLine2 =
                    AddAddressLine(
                        AddAddressLine(
                            AddAddressLine(
                                AddAddressLine(null, source.AddressLine2), source.AddressLine3), source.AddressLine4), source.AddressLine5);

                address.AddressLine1 = source.AddressLine1;
                address.AddressLine2 = addressLine2;
                address.AddressLine3 = source.Town;
                address.AddressLine4 = source.County;
                address.Postcode = source.PostCode;
                address.GeoPoint.Latitude = Convert.ToDouble(source.LatitudeSpecified ? source.Latitude : default(decimal));
                address.GeoPoint.Longitude = Convert.ToDouble(source.LongitudeSpecified ? source.Longitude : default(decimal));
            }

            return address;
        }
        private static string AddAddressLine(string addressLine, string addressLineToAdd)
        {
            if (!string.IsNullOrWhiteSpace(addressLineToAdd))
            {
                if (!string.IsNullOrWhiteSpace(addressLine))
                {
                    addressLine += ", " + addressLineToAdd;
                }
                else
                {
                    addressLine += addressLineToAdd;
                }
            }

            return addressLine;
        }
    }
}
