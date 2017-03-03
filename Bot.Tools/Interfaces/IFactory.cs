namespace Bot.Tools.Interfaces {
  public interface IFactory<in T, out TResult> {
    TResult Create(T input);
  }

  public interface IFactory<in T, in T2, out TResult> {
    TResult Create(T input, T2 input2);
  }
}
