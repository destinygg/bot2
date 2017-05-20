using Bot.Tools.Interfaces;

namespace Bot.Models.Interfaces {
  public interface ISendableVisitor<out TResult>
    : IVisitor<ISendable<PublicMessage>, TResult>
    , IVisitor<ISendable<ErrorMessage>, TResult>
    , IVisitor<ISendable<Pardon>, TResult>
    , IVisitor<ISendable<Ipban>, TResult>
    , IVisitor<ISendable<Mute>, TResult>
    , IVisitor<ISendable<Ban>, TResult> {

  }
}
