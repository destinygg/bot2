namespace Bot.Models.Contracts {
  public interface ITargetable : ITransmittable {
    IUser Target { get; }
  }
}
