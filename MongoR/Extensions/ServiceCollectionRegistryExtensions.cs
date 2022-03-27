using Microsoft.Extensions.DependencyInjection;
using MongoR.Interfaces;
using MongoR.Models;

namespace MongoR.Extensions;

public static class ServiceCollectionRegistryExtensions
{
    public static void AddMongoRCollections<TRegistry>(this IServiceCollection serviceCollection,
        ServiceLifetime repositoriesLifetime = ServiceLifetime.Scoped)
        where TRegistry : MongoDatabaseContextRegistry, IMongoDbContext
    {
        var mongoDatabase =  (TRegistry)Activator.CreateInstance(typeof(TRegistry),
            new MongoRDiContainerSettings(serviceCollection, repositoriesLifetime))!;

        serviceCollection.AddSingleton<IMongoDbContext>(mongoDatabase);
    }
}