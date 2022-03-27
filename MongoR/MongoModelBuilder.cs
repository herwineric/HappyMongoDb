using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoR.Interfaces;
using MongoR.Models;

namespace MongoR;

public sealed class MongoModelBuilder : IMongoModelBuilder
{
    private readonly IMongoDatabase _database;
    private readonly IServiceCollection? _serviceCollection;
    private readonly ServiceLifetime _globalRepositotyServiceLifetime;

    public MongoModelBuilder(IMongoDatabase database)
    {
        _database = database;

        Maps = new Dictionary<Type, MappedCollectionMetadata>();
    }

    public MongoModelBuilder(IMongoDatabase database, IServiceCollection? serviceCollection,
        ServiceLifetime globalRepositotyServiceLifetime = ServiceLifetime.Scoped)
        : this(database)
    {
        _serviceCollection = serviceCollection;
        _globalRepositotyServiceLifetime = globalRepositotyServiceLifetime;
    }

    private Dictionary<Type, MappedCollectionMetadata> Maps { get; }

    public IMongoCollection<TEntity> MongoCollection<TEntity>(out string collectionName)
        where TEntity : new()
    {
        bool mapped = Maps.TryGetValue(typeof(TEntity), out MappedCollectionMetadata? metadata);

        if (!mapped || metadata == null)
            throw new Exception($"There is no entity/document that is registered with:\n '{nameof(TEntity)}'");

        collectionName = metadata!.CollectionName;

        return _database.GetCollection<TEntity>(collectionName);
    }

    public List<MappedCollectionMetadata> GetAllRegisteredCollectionMappings() => Maps.Values.ToList();

    //TODO: Add support for document validation. FluentValidation?
    /// <summary>
    /// Provide the entity model of the document that is used in MongoDb
    /// </summary>
    /// <param name="collection"> Name of collection </param>
    /// <param name="serviceLifetimeOverride"> Override to service lifetime in Di container <see cref="ServiceLifetime"/>  </param>
    /// <typeparam name="TEntity"></typeparam>
    public void MapEntity<TEntity>(string collection, ServiceLifetime? serviceLifetimeOverride = null)
        where TEntity : new()
    {
        CheckIfCollectionAlreadyRegistered(collection);
        Maps.Add(typeof(TEntity), new MappedCollectionMetadata(typeof(TEntity), collection));

        if (_serviceCollection is not null)
        {
            if (serviceLifetimeOverride is null)
            {
                RegisterServiceCollection<TEntity>(_globalRepositotyServiceLifetime);
            }
            else
            {
                RegisterServiceCollection<TEntity>(serviceLifetimeOverride);
            }
        }
    }
    
    private void RegisterServiceCollection<TEntity>(ServiceLifetime? serviceLifetime)
        where TEntity : new()
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                _serviceCollection!.AddSingleton<IMongoRepositoryContext<TEntity>, MongoRepositoryContext<TEntity>>();
                break;
            case ServiceLifetime.Transient:
                _serviceCollection!.AddTransient<IMongoRepositoryContext<TEntity>, MongoRepositoryContext<TEntity>>();
                break;
            default:
                _serviceCollection!.AddScoped<IMongoRepositoryContext<TEntity>, MongoRepositoryContext<TEntity>>();
                break;
        }
    }

    private void CheckIfCollectionAlreadyRegistered(string collection)
    {
        bool existingMap = Maps.Values.Any(regColl => regColl.CollectionName == collection);

        if (existingMap)
            throw new Exception($"There is already a collection [{collection}] in the registry.");
    }
}