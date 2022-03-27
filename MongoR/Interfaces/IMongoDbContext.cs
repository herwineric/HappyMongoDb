using MongoDB.Driver;

namespace MongoR.Interfaces;

public interface IMongoDbContext
{
    public IMongoDatabase Database { get; }

    public string DatabaseName { get; }

    public IMongoCollection<TEntity> GetRegisteredCollection<TEntity>(out string collectionName)
        where TEntity : new();
}