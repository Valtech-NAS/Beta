namespace AddressLoader.Mongo
{
    using MongoDB.Driver;

    public class MongoAddressProvider
    {
        private readonly MongoCollection<MongoAddressWrapper> _collection;

        public MongoAddressProvider(string mongoConnectionString, string mongoDatabaseName, string mongoCollectionName)
        {
            _collection = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDatabaseName)
                .GetCollection<MongoAddressWrapper>(mongoCollectionName);
        }

        public MongoCollection<MongoAddressWrapper> Collection
        {
            get { return _collection; }
        }

        //public IEnumerable<Address> Fetch(int skip, int take)
        //{
        //    var query = Query.EQ("details.isResidential", true);

        //    return _collection
        //        .FindAs<MongoAddressWrapper>(query)
        //        .SetSkip(skip)
        //        .SetLimit(take)
        //        .Select(a => a.ToAddress())
        //        .OrderBy(a => a.Postcode)
        //        .ToList();
        //}
    }
}