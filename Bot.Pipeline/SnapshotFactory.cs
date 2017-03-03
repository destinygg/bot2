using System.Collections.Generic;
using System.Linq;
using Bot.Logic.ReceivedVisitor;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Snapshot;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class SnapshotFactory : IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>> {
    private readonly IReceivedVisitor<DelegatedSnapshotFactory> _receivedVisitor;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly List<IReceived<IUser, ITransmittable>> _context = new List<IReceived<IUser, ITransmittable>>(); // Todo: Optimization: Use a circular buffer

    public SnapshotFactory(IReceivedVisitor<DelegatedSnapshotFactory> receivedVisitor, ITimeService timeService, ISettings settings) {
      _receivedVisitor = receivedVisitor;
      _timeService = timeService;
      _settings = settings;
    }

    public ISnapshot<IUser, ITransmittable> Create(IReceived<IUser, ITransmittable> first) {
      try {
        var snapshotFactory = first.Accept(_receivedVisitor);
        return snapshotFactory.Create(_context.ToList()); // The .ToList() creates a new instance; important!
      } finally {
        _context.Insert(0, first);
        if (_context.Count > _settings.ContextSize) {
          _context.RemoveAt(_settings.ContextSize);
        }
      }
    }

    public ISnapshot<IUser, ITransmittable> OnErrorCreate => new ErrorSnapshot(new ReceivedError($"An error occured in {nameof(SnapshotFactory)}", _timeService), new List<IReceived<IUser, ITransmittable>>());
  }
}
