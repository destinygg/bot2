using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class PublicToPrivateMessageFactory : IFactory<ISendable<ITransmittable>, Moderator, ISendable<ITransmittable>> {

    public ISendable<ITransmittable> Create(ISendable<ITransmittable> input, Moderator moderator) {
      if (input is ISendable<PublicMessage>) {
        var publicMessage = (ISendable<PublicMessage>) input;
        return new SendablePrivateMessage(publicMessage.Transmission.Text, moderator);
      }
      return input;
    }
  }
}
