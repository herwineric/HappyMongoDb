using HappyMongoDb;
using HappyMongoDb.Interfaces;
using HappyMongoDb.Models;
using HappyMongoDb.Utils;
using HappyMongoDbTest.Entities;
using Xunit;

namespace HappyMongoDbTest;


public class MongoatabaseContextNoDatabaseConnection_Test : MongoDatabaseContextRegistry
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
    public void OnConfigure_NoDbConnection_Test()
    {
        Assert.True(DatabaseConfiguration.DatabaseMode == DatabaseMode.NoDatabaseConnection);
        Assert.Null(DatabaseConfiguration.ConnectionString);
        Assert.Null(DatabaseConfiguration.DatabaseName);
    }


}