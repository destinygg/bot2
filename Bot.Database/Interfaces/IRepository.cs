using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bot.Database.Interfaces {
  public interface IRepository<TEntity> where TEntity : class {
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

  }
}
