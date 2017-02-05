namespace Bot.Tools.Interfaces {
  public interface IErrorableFactory<in T, out TResult> : IFactory<T, TResult> {

    /// <summary>
    /// When the Factory's .Create() encounters a problem, this will be the fallback Create.
    /// This should return a TResult signifying an error or no action.
    /// </summary>
    TResult OnErrorCreate { get; }
  }
}
