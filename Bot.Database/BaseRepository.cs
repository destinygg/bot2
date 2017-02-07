using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : class {
    protected readonly DbSet<TEntity> Entities;

    protected BaseRepository(DbSet<TEntity> entities) {
      Entities = entities;
    }

    public IEnumerable<TEntity> GetAll() =>
      Entities.ToList();

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
      Entities.Where(predicate);

    public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate) =>
      Entities.SingleOrDefault(predicate);

    public void Add(TEntity entity) =>
      Entities.Add(entity);

    public void Add(IEnumerable<TEntity> entities) =>
      Entities.AddRange(entities);

    public void Update(TEntity entity) =>
      Entities.Update(entity);

    public void Update(IEnumerable<TEntity> entities) =>
      Entities.UpdateRange(entities);

    public void Remove(TEntity entity) =>
      Entities.Remove(entity);

    public void Remove(IEnumerable<TEntity> entities) =>
      Entities.RemoveRange(entities);
  }
}
