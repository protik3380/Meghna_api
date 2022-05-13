using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICommonManager<T> where T : class
    {
        bool Add(T entity);
        bool Add(ICollection<T> entities);

        bool Update(T entity);
        bool Update(ICollection<T> entities);

        bool Delete(T entity);
        bool Delete(ICollection<T> entities);

        ICollection<T> GetAll(params Expression<Func<T, object>>[] includes);
        ICollection<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}