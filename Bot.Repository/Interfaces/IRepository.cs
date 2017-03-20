using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bot.Repository.Interfaces {
  public interface IRepository<TModel> where TModel : class {
    IEnumerable<TModel> GetAll();
    IEnumerable<TModel> Where(Expression<Func<TModel, bool>> predicate);
    TModel SingleOrDefault(Expression<Func<TModel, bool>> predicate);

    void Add(TModel entity);
    void AddRange(IEnumerable<TModel> entities);

    void Update(TModel entity);
    void UpdateRange(IEnumerable<TModel> entities);

    void Remove(TModel entity);
    void RemoveRange(IEnumerable<TModel> entities);

  }
}
