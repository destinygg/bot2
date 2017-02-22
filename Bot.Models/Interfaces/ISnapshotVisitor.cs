using Bot.Tools.Interfaces;

namespace Bot.Models.Interfaces {
  public interface ISnapshotVisitor<out TResult>
    : IVisitor<ISnapshot<Civilian, PublicMessage>, TResult>
    , IVisitor<ISnapshot<Moderator, PublicMessage>, TResult>
    , IVisitor<ISnapshot<Moderator, ErrorMessage>, TResult>
    , IVisitor<ISnapshot<Moderator, Pardon>, TResult> {

  }
}
