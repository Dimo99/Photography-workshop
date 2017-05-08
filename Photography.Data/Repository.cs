using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Photography.Data.Interfaces;

namespace Photography.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> set;
        public Repository(DbSet<TEntity> set)
        {
            this.set = set;
        }
        public void Add(TEntity entity)
        {
            this.set.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            this.set.Remove(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.set.AddRange(entities);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            this.set.RemoveRange(entities);
        }

        public TEntity GetById(int id)
        {
            return this.set.Find(id);
        }

        public TEntity First(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.First(expression);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.FirstOrDefault(expression);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.set.Where(s => true);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.Where(expression);
        }

        public int Count()
        {
            return this.set.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.Count(expression);
        }
    }
}