namespace Bot.Tools.Interfaces {
  public interface IProvider<out T> {
    /// <summary>
    /// Gets an instance of <typeparamref name="T"/>
    /// </summary>
    /// <remarks>
    /// The implementer of this <see cref="IProvider{T}"/> is responsible for managing the lifestyle of the returned object
    /// </remarks>
    T Get();
  }
}
