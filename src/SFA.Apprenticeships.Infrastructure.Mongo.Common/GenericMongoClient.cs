namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using Domain.Interfaces.Configuration;
    using MongoDB.Driver;

    public class GenericMongoClient<T>
    {
        protected readonly MongoCollection<T> Collection;

        protected GenericMongoClient(IConfigurationManager configurationManager, string mongoConnectionSettingName,
            string mongoCollectionName)
        {
            string mongoConnectionString = configurationManager.GetAppSetting(mongoConnectionSettingName);
            string mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            Collection = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName)
                .GetCollection<T>(mongoCollectionName);
        }

        //protected IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    throw new NotImplementedException();
        //}
    }
}