namespace Bot.Tools.Interfaces {

  public interface IGenericClassFactory<in T> {
    TResult Create<TResult>(T input)
      where TResult : class;
  }

  public interface IGenericClassFactory<in T1, in T2> {
    TResult Create<TResult>(T1 input1, T2 input2)
      where TResult : class;
  }

  public interface IGenericClassFactory<in T1, in T2, in T3> {
    TResult Create<TResult>(T1 input1, T2 input2, T3 input3)
      where TResult : class;
  }
}
