using MongoR;
using MongoR.Interfaces;
using MongoR.Models;
using MongoR.Utils;
using Xunit;

namespace MongoR_Test;

public class MongoatabaseContextNoDatabaseConnectionTest : MongoDatabaseContextRegistry
{
    protected override void ModelBuilder(IMongoModelBuilder mapper)
    {
    }

    protected override void OnConfigure(MongoDatabaseConfiguration configuration)
    {
        configuration.NoDatabaseConnection();

        base.OnConfigure(configuration);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_NoDatabaseConnectionMode_Test()
    {
        Assert.True(DatabaseConfiguration.DatabaseMode == DatabaseMode.NoDatabaseConnection);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_ConnectionString_Test()
    {
        Assert.Null(DatabaseConfiguration.ConnectionString);
    }

    [Fact]
    public void OnConfigure_HasDbConnection_DatabaseName_Test()
    {
        Assert.Null(DatabaseConfiguration.DatabaseName);
    }
}