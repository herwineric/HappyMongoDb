using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoR.Models;

namespace MongoR.Interfaces;

public interface IMongoModelBuilder
{
    public IMongoCollection<TEntity> MongoCollection<TEntity>(out string collectionName)
        where TEntity : new();

    public void MapEntity<TEntity>(string collection, ServiceLifetime? serviceLifetimeOverride = null)
        where TEntity : new();

    public List<MappedCollectionMetadata> GetAllRegisteredCollectionMappings();
}