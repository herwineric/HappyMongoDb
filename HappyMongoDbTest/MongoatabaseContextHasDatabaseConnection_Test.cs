using HappyMongoDb;
using HappyMongoDb.Interfaces;
using HappyMongoDb.Models;
using HappyMongoDbTest.Entities;
using MongoDB.Driver;
using Xunit;

namespace HappyMongoDbTest;

public class MongoatabaseContextHasDatabaseConnection_Test : MongoDatabaseContextRegistry
{
    private const string ConnString = "mongodb://mongodb0.example.com:27017";
    private const string DbName = "DbName";
    private const string CollectionNametest = "collection";

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

    [Fact]
    public void OnConfigure_HasDbConnection_Test()
    {
        Assert.True(DatabaseConfiguration.DatabaseMode == default);
        Assert.Equal(ConnString, DatabaseConfiguration.ConnectionString);
        Assert.Equal(DbName, DatabaseConfiguration.DatabaseName);

        var collection = GetRegisteredCollection<MockDatabaseEntity>(out string colName);
        Assert.IsAssignableFrom<IMongoCollection<MockDatabaseEntity>>(collection);
        Assert.Equal(CollectionNametest, colName);
    }
}