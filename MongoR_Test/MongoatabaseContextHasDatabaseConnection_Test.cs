using MongoDB.Driver;
using MongoR;
using MongoR.Interfaces;
using MongoR.Models;
using MongoR_Test.Entities;
using Xunit;

namespace MongoR_Test;

public class MongoatabaseContextHasDatabaseConnectionTest : MongoDatabaseContextRegistry
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
    public void OnConfigure_HasDbConnection_DefaultMode_Test()
    {
        Assert.True(DatabaseConfiguration.DatabaseMode == default);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_ConnectionString_Test()
    {
        Assert.Equal(ConnString, DatabaseConfiguration.ConnectionString);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_DatabaseName_Test()
    {
        Assert.Equal(DbName, DatabaseConfiguration.DatabaseName);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_MockDatabaseEntityCollection_Test()
    {
        var collection = GetRegisteredCollection<MockDatabaseEntity>(out string colName);
        Assert.IsAssignableFrom<IMongoCollection<MockDatabaseEntity>>(collection);
        Assert.Equal(CollectionNametest, colName);
    }
}