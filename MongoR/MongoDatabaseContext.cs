using MongoDB.Driver;
using MongoR.Interfaces;

namespace MongoR;

public abstract class MongoDatabaseContext
{
    protected MongoDatabaseContext(IMongoDbContext context)
    {
        Database = context.Database;
    }

    public IMongoDatabase Database { get; }
}