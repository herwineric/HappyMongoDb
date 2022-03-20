using MongoDB.Driver;

namespace HappyMongoDb.Interfaces;

public interface IMongoModelBuilder
{
    public IMongoCollection<TEntity> MongoCollection<TEntity>(out string collectionName)
        where TEntity : IEntityModel, new();

    public void MapEntity<TEntity>(string collection)
        where TEntity : IEntityModel, new();
}