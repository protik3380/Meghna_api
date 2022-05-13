using EFreshStoreCore.Model.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace EFreshStoreCore.Repository
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        protected DbContext Db;

        public CommonRepository(DbContext db)
        {
            Db = db;
        }

        protected DbSet<T> Table
        {
            get { return Db.Set<T>(); }
        }

        public bool Add(T entity)
        {
            Table.Add(entity);
            int rowAffected = Db.SaveChanges();
            return rowAffected > 0;
        }

        public bool Add(ICollection<T> entities)
        {
            Table.AddRange(entities);
            int rowAffected = Db.SaveChanges();
            return rowAffected > 0;
        }

        public bool Update(T entity)
        {
            Table.AddOrUpdate(entity);
            return Db.SaveChanges() > 0;
        }

        public bool Update(ICollection<T> entities)
        {
            Table.AddOrUpdate(entities.ToArray());
            return Db.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            Table.Attach(entity);
            Table.Remove(entity);
            return Db.SaveChanges() > 0;
        }

        public bool Delete(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                Table.Attach(entity);
            }
            Table.RemoveRange(entities);
            return Db.SaveChanges() > 0;
        }

        public ICollection<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return includes
                .Aggregate(
                   Table.AsNoTracking().AsQueryable(),
                    (current, include) => current.Include(include)
                ).ToList();
        }

        public ICollection<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return includes
              .Aggregate(
                  Table.AsNoTracking().AsQueryable(),
                  (current, include) => current.Include(include),
                 c => c.Where(predicate)
              ).ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return includes
              .Aggregate(
                  Table.AsNoTracking().AsQueryable(),
                  (current, include) => current.Include(include),
                 c => c.FirstOrDefault(predicate)
              );
        }
    }
}