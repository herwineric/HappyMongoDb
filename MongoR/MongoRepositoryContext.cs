using System.Linq.Expressions;
using MongoDB.Driver;
using MongoR.Extensions;
using MongoR.Interfaces;

namespace MongoR;

public class MongoRepositoryContext<TEntity> : MongoDatabaseContext, IMongoRepositoryContext<TEntity>
    where TEntity : new()
{
    public MongoRepositoryContext(IMongoDbContext context)
        : base(context)
    {
        Collection = context.GetRegisteredCollection<TEntity>(out string collection);

        CollectionName = collection;
        DatabaseName = context.DatabaseName;
    }

    public IMongoCollection<TEntity> Collection { get; }

    public string CollectionName { get; }

    public string DatabaseName { get; }

    public TEntity? GetOne(Expression<Func<TEntity, bool>> exp) => Collection.GetBy(exp);

    public async Task<IEnumerable<TEntity>> GetAsync() => await Collection.GetAllAsync();

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp) =>
        await Collection.GetAllAsync(exp);

    public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> exp) => await Collection.GetByAsync(exp);

    public async Task<TEntity> InsertAsync(TEntity entity) => await Collection.InsertAsync(entity);

    public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> exp) => await Collection.DeleteAsync(exp);


    public async Task<TEntity> ReplaceAsync(Expression<Func<TEntity, bool>> exp, TEntity entity) =>
        await Collection.ReplaceAsync(exp, entity);

    public async Task<TEntity> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> exp, TEntity entity)
    {
        await Collection.ReplaceOneAsync(exp, entity, new ReplaceOptions { IsUpsert = true });
        return entity;
    }

    public async Task SessionInsertAsync(IClientSessionHandle session, TEntity entity)
    {
        var sessionCollection = SessionCollection(session);
        await sessionCollection.InsertOneAsync(entity);
    }

    public async Task SessionReplaceAsync(IClientSessionHandle session, Expression<Func<TEntity, bool>> exp,
        TEntity entity)
    {
        var sessionCollection = SessionCollection(session);
        await sessionCollection.ReplaceOneAsync(exp, entity);
    }

    public async Task SessionInsertOrUpdateAsync(IClientSessionHandle session, Expression<Func<TEntity, bool>> exp,
        TEntity entity)
    {
        var sessionCollection = SessionCollection(session);
        await sessionCollection.ReplaceOneAsync(exp, entity, new ReplaceOptions { IsUpsert = true });
    }


    private IMongoCollection<TEntity> SessionCollection(IClientSessionHandle session) =>
        session.Client.GetDatabase(DatabaseName).GetCollection<TEntity>(CollectionName);
}