using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class SendablesFactory : IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly ISendableGenerator _sendableGenerator;

    public SendablesFactory(ISendableGenerator sendableGenerator) {
      _sendableGenerator = sendableGenerator;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, ITransmittable> snapshot) => _sendableGenerator.Generate(snapshot);
    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new List<ISendable<ITransmittable>>();
  }
}
