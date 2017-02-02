namespace Bot.Models.Interfaces {
  public interface IUserVisitor {
    IReceivedVisitor Visit(Moderator moderator);
    IReceivedVisitor Visit(Civilian civilian);
  }
}
