using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using VacancyDetailProxy;
    using Domain.Entities.Locations;

    public class VacancyDetailAddressResolver : ValueResolver<AddressData, Address>
    {
        protected override Address ResolveCore(AddressData source)
        {
            var address = new Address { GeoPoint = new GeoPoint() };

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
                address.GeoPoint.Latitude = (double)source.Latitude.GetValueOrDefault();
                address.GeoPoint.Longitude = (double)source.Longitude.GetValueOrDefault();
            }

            return address;
        }
        private string AddAddressLine(string addressLine, string addressLineToAdd)
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
