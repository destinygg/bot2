namespace Bot.Models.Interfaces {
  public interface ISendable<out T>
    where T : ITransmittable {
    T Transmission { get; }
  }
}
