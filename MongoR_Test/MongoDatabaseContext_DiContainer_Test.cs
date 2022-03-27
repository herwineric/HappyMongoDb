using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoR;
using MongoR.Extensions;
using MongoR.Interfaces;
using MongoR.Models;
using MongoR_Test.Entities;
using Xunit;

namespace MongoR_Test;

public class MongoDatabaseContextDiContainerTestObject : MongoDatabaseContextRegistry
{
    public const string ConnString = "mongodb://mongodb0.example.com:27017";
    public const string DbName = "DbName";
    public const string CollectionNametest = "collection";

    public MongoDatabaseContextDiContainerTestObject(MongoRDiContainerSettings containerSettings)
        : base(containerSettings)
    {
    }

    protected override void ModelBuilder(IMongoModelBuilder mapper)
    {
        mapper.MapEntity<MockDatabaseEntity>(CollectionNametest);
    }

    protected override void OnConfigure(MongoDatabaseConfiguration configuration)
    {
        configuration.ConnectionString = ConnString;
        configuration.DatabaseName = DbName;

        base.OnConfigure(configuration);
    }
}

public class MongoDatabaseContext_DiContainer_Test
{
    private readonly IServiceProvider _provider;

    public MongoDatabaseContext_DiContainer_Test()
    {
        var services = new ServiceCollection();

        services.AddMongoRCollections<MongoDatabaseContextDiContainerTestObject>();

        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void GetService_MongoDatabaseContextRegistry_Test()
    {
        using IServiceScope scope = _provider.CreateScope();
        var registryContext = _provider.GetService<IMongoDbContext>();

        var collection = registryContext.GetRegisteredCollection<MockDatabaseEntity>(out string collectionName);

        Assert.IsAssignableFrom<IMongoCollection<MockDatabaseEntity>>(collection);
        Assert.Equal(MongoDatabaseContextDiContainerTestObject.CollectionNametest, collectionName);
    }

    [Fact]
    public void GetService_RepositoryContextEntity_Test()
    {
        using IServiceScope scope = _provider.CreateScope();
        var entityContext = scope.ServiceProvider.GetService<IMongoRepositoryContext<MockDatabaseEntity>>();

        Assert.IsAssignableFrom<IMongoRepositoryContext<MockDatabaseEntity>>(entityContext);
        Assert.Equal(MongoDatabaseContextDiContainerTestObject.CollectionNametest, entityContext.CollectionName);
    }
}