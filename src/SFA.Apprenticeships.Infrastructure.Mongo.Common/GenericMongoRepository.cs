namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using System;
    using Infrastructure.Common.Configuration;
    using MongoDB.Driver;

    public class GenericMongoRepository<T>
    {
        protected readonly MongoCollection<T> Collection;

        protected GenericMongoRepository(IConfigurationManager configurationManager, string mongoConnectionSettingName,
            string mongoDbName, string mongoCollectionName)
        {
            Collection = new MongoClient(configurationManager.GetAppSetting(mongoConnectionSettingName))
                .GetServer()
                .GetDatabase(mongoDbName)
                .GetCollection<T>(mongoCollectionName);
        }

        //protected IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    //todo: implement predicated Get in generic mongo repo... or should move to concretes...?
        //    throw new NotImplementedException();
        //}
    }
}
