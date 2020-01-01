using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankBros.Backend.Core.Entities;

namespace BankBros.Backend.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        bool Add(params T[] items);
        bool Update(params T[] items);
        bool Remove(params T[] items);
    }
}
