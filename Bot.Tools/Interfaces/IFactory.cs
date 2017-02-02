namespace Bot.Tools.Interfaces {
  public interface IFactory<in T, out TResult> {
    TResult Create(T input);
  }
}
