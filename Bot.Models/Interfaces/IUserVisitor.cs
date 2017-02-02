namespace Bot.Models.Interfaces {
  public interface IUserVisitor<out TResult> {
    TResult Visit(Moderator moderator);
    TResult Visit(Civilian civilian);
  }
}
