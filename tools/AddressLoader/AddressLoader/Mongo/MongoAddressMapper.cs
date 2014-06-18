using System;
using AddressLoader.Domain;

namespace AddressLoader.Mongo
{
    public static class MongoAddressMapper
    {
        public static Address ToAddress(this MongoAddressWrapper mongoAddress)
        {
            return new Address
            {
                AddressLine1 =
                    string.IsNullOrWhiteSpace(mongoAddress.Address.AddressLine1)
                        ? mongoAddress.Address.AddressLine2
                        : mongoAddress.Address.AddressLine1,
                AddressLine2 =
                    string.IsNullOrWhiteSpace(mongoAddress.Address.AddressLine1)
                        ? string.Empty
                        : mongoAddress.Address.AddressLine2,
                AddressLine3 = mongoAddress.Address.AddressLine3,
                AddressLine4 = mongoAddress.Address.AddressLine4,
                Postcode = mongoAddress.Address.Postcode,
                Location = new Location
                {
                    Longitude = mongoAddress.Location.Longitude,
                    Latitude = mongoAddress.Location.Latitude
                }
            };
        }
    }
}
