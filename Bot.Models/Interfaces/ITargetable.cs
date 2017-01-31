namespace Bot.Models.Interfaces {
  public interface ITargetable : ITransmittable {
    IUser Target { get; }
  }
}
