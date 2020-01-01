using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BankBros.Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using EntityState = BankBros.Backend.Core.Entities.EntityState;

namespace BankBros.Backend.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
  where TEntity : class, IEntity, new()
  where TContext : DbContext, new()
    {
        public virtual IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;
            using (var context = new TContext())
            {
                IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<TEntity>();
            }

            return list;
        }

        public virtual IList<TEntity> GetList(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;
            using (var context = new TContext())
            {
                IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<TEntity>();
            }

            return list;
        }

        public virtual TEntity GetSingle(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            TEntity item = null;
            using (var context = new TContext())
            {
                IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual bool Add(params TEntity[] items)
        {
            foreach (var entity in items)
            {
                entity.EntityState = EntityState.Added;
            }

            return Update(items);
        }

        public virtual bool Update(params TEntity[] items)
        {
            foreach (var entity in items)
            {
                entity.EntityState = entity.EntityState == EntityState.Unchanged ? EntityState.Modified : entity.EntityState;
            }
            using (var context = new TContext())
            {
                DbSet<TEntity> dbSet = context.Set<TEntity>();
                foreach (TEntity item in items)
                {
                    dbSet.Add(item);
                    foreach (EntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
                    {
                        IEntity entity = entry.Entity;
                        entry.State = GetEntityState(entity.EntityState);
                    }
                }

                context.SaveChanges();
            }

            return true;
        }

        public virtual bool Remove(params TEntity[] items)
        {
            foreach (var entity in items)
            {
                entity.EntityState = EntityState.Deleted;
            }
            return Update(items);
        }

        // EntityStater this method switches entitystates to make real methods above
        protected static Microsoft.EntityFrameworkCore.EntityState GetEntityState(Core.Entities.EntityState entityState)
        {
            switch (entityState)
            {
                case Core.Entities.EntityState.Unchanged:
                    return Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                case Core.Entities.EntityState.Added:
                    return Microsoft.EntityFrameworkCore.EntityState.Added;
                case Core.Entities.EntityState.Modified:
                    return Microsoft.EntityFrameworkCore.EntityState.Modified;
                case Core.Entities.EntityState.Deleted:
                    return Microsoft.EntityFrameworkCore.EntityState.Deleted;
                default:
                    return Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }
    }
}
