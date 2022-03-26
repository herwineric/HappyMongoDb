using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoR.Extensions;

public static class MongoCollectionExtensions
{
    public static IEnumerable<TEntity> GetAll<TEntity>(this IMongoCollection<TEntity> collection)
    {
        return collection.Find(ent => true).ToList();
    }

    public static TEntity? GetBy<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp) => collection.FindSync(exp).SingleOrDefault();

    public static async Task<List<TEntity>> GetAllAsync<TEntity>(this IMongoCollection<TEntity> collection)
    {
        return await collection.Find(ent => true).ToListAsync();
    }

    public static async Task<List<TEntity>> GetAllAsync<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp)
    {
        return await collection.Find(exp).ToListAsync();
    }
    
    public static async Task<TEntity?> GetByAsync<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp) => await collection.Find(exp).SingleOrDefaultAsync();

    public static async Task<TEntity> InsertAsync<TEntity>(this IMongoCollection<TEntity> collection, TEntity entity)
    {
        await collection.InsertOneAsync(entity);
        return entity;
    }

    public static async Task<TEntity> DeleteAsync<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp) => await collection.FindOneAndDeleteAsync(exp);


    public static async Task<TEntity> ReplaceAsync<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp, TEntity entity)
    {
        await collection.ReplaceOneAsync(exp, entity);
        return entity;
    }
    
    public static async Task<TEntity> InsertOrUpdateAsync<TEntity>(this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> exp, TEntity entity)
    {
        await collection.ReplaceOneAsync(exp, entity, new ReplaceOptions { IsUpsert = true });
        return entity;
    }
}