namespace Bot.Models.Interfaces {
  public interface IDynamicVisitor<out TResult> {
    TResult DynamicVisit(dynamic input);
  }
}
