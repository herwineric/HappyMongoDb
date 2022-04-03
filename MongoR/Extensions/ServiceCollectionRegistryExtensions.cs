using Microsoft.Extensions.DependencyInjection;
using MongoR.Interfaces;
using MongoR.Models;

namespace MongoR.Extensions;

public static class ServiceCollectionRegistryExtensions
{
    public static void AddMongoRCollections<TRegistry>(this IServiceCollection serviceCollection,
        string dbConnectionString,  ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRegistry : MongoDatabaseContextRegistry, IMongoDbContext
    {
        var settings = new MongoRDiContainerSettings
        {
            ServiceCollection = serviceCollection, DbConnectionString =
                dbConnectionString,
            Lifetime = lifetime
        };
        
        var mongoDatabase =  (TRegistry)Activator.CreateInstance(typeof(TRegistry), settings)!;

        serviceCollection.AddSingleton<IMongoDbContext>(mongoDatabase);
    }
}