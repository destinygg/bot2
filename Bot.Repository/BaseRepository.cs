using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public abstract class BaseRepository<TModel> : IRepository<TModel>
    where TModel : class {
    protected readonly DbSet<TModel> Entities;

    protected BaseRepository(DbSet<TModel> entities) {
      Entities = entities;
    }

    public IEnumerable<TModel> GetAll() =>
      Entities.ToList();

    public IEnumerable<TModel> Where(Expression<Func<TModel, bool>> predicate) =>
      Entities.Where(predicate);

    public virtual TModel SingleOrDefault(Expression<Func<TModel, bool>> predicate) =>
      Entities.SingleOrDefault(predicate);

    public void Add(TModel entity) =>
      Entities.Add(entity);

    public void AddRange(IEnumerable<TModel> entities) =>
      Entities.AddRange(entities);

    public void Update(TModel entity) =>
      Entities.Update(entity);

    public void UpdateRange(IEnumerable<TModel> entities) =>
      Entities.UpdateRange(entities);

    public void Remove(TModel entity) =>
      Entities.Remove(entity);

    public void RemoveRange(IEnumerable<TModel> entities) =>
      Entities.RemoveRange(entities);
  }
}
