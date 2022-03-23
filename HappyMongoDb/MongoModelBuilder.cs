using HappyMongoDb.Interfaces;
using MongoDB.Driver;

namespace HappyMongoDb;

public sealed class MongoModelBuilder : IMongoModelBuilder
{
    private readonly IMongoDatabase _database;

    public MongoModelBuilder(IMongoDatabase database)
    {
        _database = database;
        Maps = new Dictionary<Type, string>();
    }

    private Dictionary<Type, string> Maps { get; }
    
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

        return _database.GetCollection<TEntity>(collection);
    }

    private void CheckIfCollectionAlreadyRegistered(string collection)
    {
        bool existingMap = Maps.Values.Any(regColl => regColl == collection);

        if (existingMap)
            throw new Exception($"There is already a collection [{collection}] in the registry.");
    }
}