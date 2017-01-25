using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bot.Database.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bot.Api {
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> Entities;

    public Repository(DbContext context) {
      Context = context;
      Entities = Context.Set<TEntity>();
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
