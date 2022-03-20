using HappyMongoDb.Interfaces;
using MongoDB.Driver;

namespace HappyMongoDb;

public sealed class MongoModelBuilder : IMongoModelBuilder
{
    public MongoModelBuilder(IMongoDbContext client)
    {
        Maps = new Dictionary<Type, string>();
        Client = client;
    }

    private Dictionary<Type, string> Maps { get; }

    private IMongoDbContext Client { get; }

    //TODO: Add support for document validation. FluentValidation?
    public void MapEntity<TEntity>(string collection)
        where TEntity : IEntityModel, new()
    {
        CheckIfCollectionAlreadyRegistered(collection);
        Maps.Add(typeof(TEntity), collection);
    }

    public IMongoCollection<TEntity> MongoCollection<TEntity>(out string collectionName)
        where TEntity : IEntityModel, new()
    {
        bool mapped = Maps.TryGetValue(typeof(TEntity), out string? collection);

        if (!mapped || collection == null)
            throw new Exception($"There is no entity/document that is registered with:\n '{nameof(TEntity)}'");

        collectionName = collection!;

        return Client.Database.GetCollection<TEntity>(collection);
    }

    private void CheckIfCollectionAlreadyRegistered(string collection)
    {
        bool existingMap = Maps.Values.Any(regColl => regColl == collection);

        if (existingMap)
            throw new Exception($"There is already a collection [{collection}] in the registry.");
    }
}