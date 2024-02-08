using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region "Sync Methods"
        TEntity Get(int id);
        TEntity Get(long id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        #endregion

        #region "Async Methods"
        Task AddAsync(TEntity entity);
        Task AddSaveAsync(TEntity entity);
        Task AddSaveRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateSaveAsync(TEntity entity);
        Task UpdateModifiedSaveAsync(TEntity entity);
        Task UpdateRangeSaveAsync(List<TEntity> entities); //IEnumerable<TEntity> entities
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(int id);

        Task UpdateInvoiceAsync(TEntity entity);
         

        #endregion 
    }
}
