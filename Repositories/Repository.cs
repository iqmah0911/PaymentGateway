using Microsoft.EntityFrameworkCore;
//using PaymentGateway.Data;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDBContext Context;

        public Repository(ApplicationDBContext context)
        {
            Context = context;
        }

        #region "Sync Methods"
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public TEntity Get(long id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>()
                .ToList();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.Entry(entities).State = EntityState.Modified;
        }
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        #endregion

        #region "Async Methods"
        public async Task<TEntity> Get(dynamic id)
        {
            return await Context.Set<TEntity>().Find(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddSaveAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task AddSaveRangeAsync(IEnumerable<TEntity> entities)

        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public void UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateSaveAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

        }

        public async Task UpdateModifiedSaveAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

        }

        public async Task UpdateInvoiceAsync(TEntity entity)
        {  
             await Context.SaveChangesAsync(); 
        }


       
        public async Task UpdateRangeSaveAsync(List<TEntity> entities)  //IEnumerable<TEntity> entities
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
              
            }
            await Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await Context.Set<TEntity>().Remove(entity).ReloadAsync();
        }

        public void RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        #endregion
    }
}
