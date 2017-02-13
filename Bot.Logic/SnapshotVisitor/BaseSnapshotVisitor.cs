using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.SnapshotVisitor {
  public abstract class BaseSnapshotVisitor<TUser> : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>
    where TUser : IUser {
    private readonly ILogger _logger;

    protected BaseSnapshotVisitor(ILogger logger) {
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit<TVisitedUser, TTransmission>(ISnapshot<TVisitedUser, TTransmission> received) // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable => DynamicVisit(received as dynamic) ?? new List<ISendable<ITransmittable>>();

    public IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(dynamic received) {
      try {
        return _DynamicVisit(received);
      } catch (RuntimeBinderException e) {
        _logger.LogError(e, $"{nameof(BaseSnapshotVisitor<IUser>)} did not handle this type: {received.GetType()}");
        return null;
      }
    }

    protected abstract IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<TUser, PublicMessage> snapshot);
    protected abstract IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<TUser, PrivateMessage> snapshot);

    private IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<IUser, ITransmittable> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}
