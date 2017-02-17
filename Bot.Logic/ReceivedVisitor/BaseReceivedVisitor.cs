using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.ReceivedVisitor {
  public abstract class BaseReceivedVisitor<TUser> : IReceivedVisitor<DelegatedSnapshotFactory>
    where TUser : IUser {
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;

    protected BaseReceivedVisitor(ILogger logger, ITimeService timeService) {
      _logger = logger;
      _timeService = timeService;
    }

    public DelegatedSnapshotFactory Visit<TVisitedUser, TTransmission>(Received<TVisitedUser, TTransmission> received) // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable => DynamicVisit(received as dynamic) ?? new DelegatedSnapshotFactory(_ => new Snapshot<IUser, ITransmittable>(new ReceivedError("Placeholder for an error", _timeService.UtcNow), new List<IReceived<IUser, ITransmittable>>()));

    public DelegatedSnapshotFactory DynamicVisit(dynamic received) {
      try {
        return _DynamicVisit(received);
      } catch (RuntimeBinderException e) {
        _logger.LogError($"{nameof(BaseReceivedVisitor<IUser>)} did not handle this type: {received.GetType()}", e);
        return null;
      }
    }

    protected abstract DelegatedSnapshotFactory _DynamicVisit(Received<TUser, PublicMessage> received);

    protected DelegatedSnapshotFactory NewSnapshotFactory(Func<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> create) => new DelegatedSnapshotFactory(create);
  }
}
