using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Manager
{
    public class CommonManager<T> : ICommonManager<T> where T : class
    {
        public ICommonRepository<T> Repository;

        public CommonManager(ICommonRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual bool Add(T entity)
        {
            return Repository.Add(entity);
        }

        public virtual bool Add(ICollection<T> entities)
        {
            return Repository.Add(entities);
        }

        public virtual bool Update(T entity)
        {
            return Repository.Update(entity);
        }

        public virtual bool Update(ICollection<T> entities)
        {
            return Repository.Update(entities);
        }

        public virtual bool Delete(T entity)
        {
            return Repository.Delete(entity);
        }

        public virtual bool Delete(ICollection<T> entities)
        {
            return Repository.Delete(entities);
        }

        public virtual ICollection<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return Repository.GetAll(includes);
        }

        public virtual ICollection<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return Repository.Get(predicate, includes);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return Repository.GetFirstOrDefault(predicate, includes);
        }
    }
}
