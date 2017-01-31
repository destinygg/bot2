using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Pipeline.Interfaces;

namespace Bot.Pipeline {
  public class SnapshotToSendable : ISnapshotToSendable {
    private readonly ISendableGenerator _sendableGenerator;

    public SnapshotToSendable(ISendableGenerator sendableGenerator) {
      _sendableGenerator = sendableGenerator;
    }

    public IReadOnlyList<ISendable> GetSendables(ISnapshot<IUser, ITransmittable> snapshot) => _sendableGenerator.Generate(snapshot);
  }
}
