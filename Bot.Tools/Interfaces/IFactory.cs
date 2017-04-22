namespace Bot.Tools.Interfaces {
  public interface IFactory<in T, out TResult> {
    TResult Create(T input);
  }

  public interface IFactory<in T1, in T2, out TResult> {
    TResult Create(T1 input1, T2 input2);
  }

  public interface IFactory<in T1, in T2, in T3, out TResult> {
    TResult Create(T1 input1, T2 input2, T3 input3);
  }
}
