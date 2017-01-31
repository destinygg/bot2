using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bot.Api.Interfaces {
  public interface IRepository<TEntity> where TEntity : class {
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);
    void Add(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void Update(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void Remove(IEnumerable<TEntity> entities);

  }
}
