namespace Bot.Tools.Interfaces {
  public interface IVisitor<in T, out TResult> {
    TResult Visit(T t);
  }
}
