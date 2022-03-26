using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoR.Interfaces
{
    public interface IMongoRepositoryContext<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAsync();

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp);

        public TEntity? GetOne(Expression<Func<TEntity, bool>> exp);
        public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> exp);

        public Task<TEntity> InsertAsync(TEntity entity);
        
        public Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> exp);

        public Task<TEntity> ReplaceAsync(Expression<Func<TEntity, bool>> exp, TEntity entity);
        
        public Task<TEntity> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> exp, TEntity entity);

        public Task SessionInsertAsync(IClientSessionHandle session, TEntity entity);

        public Task SessionReplaceAsync(IClientSessionHandle session, Expression<Func<TEntity, bool>> exp,
            TEntity entity);

        public Task SessionInsertOrUpdateAsync(IClientSessionHandle session, Expression<Func<TEntity, bool>> exp,
            TEntity entity);
    }
}