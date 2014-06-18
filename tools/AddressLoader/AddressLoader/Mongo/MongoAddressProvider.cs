using System;
using System.Collections.Generic;
using System.Linq;
using AddressLoader.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AddressLoader.Mongo
{
    public class MongoAddressProvider
    {
        private readonly MongoCollection<MongoAddress> _collection;

        public MongoAddressProvider(string mongoConnectionString, string mongoDatabaseName, string mongoCollectionName)
        {
            _collection = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDatabaseName)
                .GetCollection<MongoAddress>(mongoCollectionName);
        }

        public IEnumerable<Address> Fetch(int skip, int take)
        {
            var query = Query.EQ("details.isResidential", true);

            return _collection
                .FindAs<MongoAddressWrapper>(query)
                .SetSkip(skip)
                .SetLimit(take)
                .Select(a => a.ToAddress())
                .OrderBy(a => a.Postcode)
                .ToList();
        }
    }
}
