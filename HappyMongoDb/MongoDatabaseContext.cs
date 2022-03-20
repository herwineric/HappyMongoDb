using HappyMongoDb.Interfaces;
using MongoDB.Driver;

namespace HappyMongoDb;

public abstract class MongoDatabaseContext
{
    protected MongoDatabaseContext(IMongoDbContext context)
    {
        Database = context.Database;
    }

    public IMongoDatabase Database { get; }
}