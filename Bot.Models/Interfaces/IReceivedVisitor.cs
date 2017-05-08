using Bot.Tools.Interfaces;

namespace Bot.Models.Interfaces {
  public interface IReceivedVisitor<out TResult>
    : IVisitor<IReceived<Civilian, PublicMessage>, TResult>
    , IVisitor<IReceived<Moderator, PublicMessage>, TResult>
    , IVisitor<IReceived<Moderator, ErrorMessage>, TResult>
    , IVisitor<IReceived<Moderator, Pardon>, TResult>
    , IVisitor<IReceived<Moderator, InitialUsers>, TResult> {

  }
}
